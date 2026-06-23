# Hephaestus UI

Hephaestus UI is a part of the **Hephaestus Core** framework that helps operate with UI in Unity.
It provides a layered, [Zenject/Extenject](https://github.com/Mathijs-Bakker/Extenject)-driven UI manager that creates
and manages widgets (screens, popups, HUD elements) from a configurable library, with a clean MVC-style split between
the **view** (`BaseUIWidget`) and its **controller** (`WidgetControllerWithData`).

## Features

- **Layered UI** — widgets are placed into ordered `UILayer`s (e.g. `Background`, `Windows`, `Popups`, `System`).
- **Data-driven widgets** — create a widget by enum type and pass a typed `WidgetData` payload to its controller.
- **DI-first** — the UI manager and its widgets are instantiated through the Zenject container, so widgets receive
  full dependency injection.
- **ScriptableObject configuration** — canvas, camera, scaler, layers and the widget library are all configured via
  `UIManagerConfig` and `WidgetsLibrary` assets.
- **Optional shared instance** — keep the UI alive across scene loads (`DontDestroyOnLoad`) with automatic URP camera
  stacking.
- **Editor assistant** — generate the View / Controller / Model / assembly definition for a new widget from a menu.

## Requirements

- Unity **2019.2+**
- [Extenject (Zenject)](https://github.com/Mathijs-Bakker/Extenject) `9.2.0` (declared as a package dependency)

Optional define symbols picked up by the package:

| Symbol             | Effect                                              |
|--------------------|-----------------------------------------------------|
| `USE_URP`          | Enables URP camera stacking in `UIManagerHandler`.  |
| `USE_INPUT_SYSTEM` | Uses `InputSystemUIInputModule` for the EventSystem.|

> A layer named **`UI`** must exist in *Project Settings → Tags and Layers*. If it is missing the manager logs an error
> and skips layer/culling assignment.

## Installation

Add the package to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.wtfgames.hephaestus.ui": "2.2.1"
  }
}
```

## Getting started

### 1. Create the configuration assets

Use the project's create menu:

- `HephaestusMobile/Core/UI/UIManagerConfig` — render mode, canvas scaler, camera and the list of layers.
- `HephaestusMobile/Core/UI/WidgetsLibrary` — maps each widget enum type to a prefab and a layer index.
- `HephaestusMobile/Core/UI/WidgetsLibraryConstants` — holds the generated enum keys.

Assign the `WidgetsLibrary` to the `UIManagerConfig`.

### 2. Install the bindings

Add the config installer (`HephaestusUIManagerSOInstaller`, created via
`HephaestusMobile/Core/UI/HephaestusUIManagerSOInstaller`) to your `SceneContext`/`ProjectContext`, and install the
manager bindings from your own installer:

```csharp
public override void InstallBindings()
{
    HephaestusUIManagerInstaller.Install(Container);
}
```

`HephaestusUIManagerInstaller` binds:

- `WidgetFactory` (a Zenject `PlaceholderFactory<GameObject, IWidget>` backed by `CustomWidgetFactory`),
- `UIManagerHandler` (created on a new GameObject by the container),
- `IUIManager` → `UIManager` (as a singleton).

### 3. Use the manager

Inject `IUIManager` anywhere and drive widgets by their enum type:

```csharp
public class GameFlow
{
    private readonly IUIManager _ui;

    public GameFlow(IUIManager ui) => _ui = ui;

    public void OpenMainMenu()
    {
        _ui.CreateUiWidgetWithData(WidgetType.MainMenu, new MainMenuModel { /* ... */ }, animate: true);
    }
}
```

## API overview (`IUIManager`)

| Method                                                                | Description                                                        |
|-----------------------------------------------------------------------|--------------------------------------------------------------------|
| `CreateUiWidgetWithData(Enum type, object data, animate, allowDuplicates)` | Instantiates a widget, initializes its controller with `data`, then activates it. Returns the `IWidget`. |
| `ActivateWidgetByType(Enum type, animated)`                           | Activates the most recent widget of that type.                     |
| `DeactivateWidgetByType(Enum type, animated)`                         | Deactivates the most recent widget of that type.                   |
| `DismissWidgetByType(Enum type)`                                      | Dismisses (destroys) the most recent widget of that type.          |
| `DismissAllWidgets()`                                                 | Dismisses every widget across all layers.                          |
| `DismissWidgetsInLayer(int layerIndex)`                              | Dismisses every widget in the given layer.                         |

When `allowDuplicates` is `true`, multiple widgets of the same type can coexist in a layer. The type-based
methods above then operate on the most recently created instance — for fine-grained control keep the `IWidget`
reference returned by `CreateUiWidgetWithData`.

## Authoring a widget

A widget consists of three pieces in the same namespace:

- **View** — a `MonoBehaviour : BaseUIWidget` on the prefab root (with a `CanvasGroup`). It owns the lifecycle
  (`Create`/`Activate`/`Deactivate`/`Dismiss`) and the fade animation (`fadeDuration` is serialized in the inspector).
- **Model** — a `class : WidgetData` carrying the payload passed to `CreateUiWidgetWithData`.
- **Controller** — a `MonoBehaviour : WidgetControllerWithData<TWidget, TData>` on the same prefab root. It receives
  the typed widget and data and reacts to the lifecycle via `OnWidgetCreated/Activated/Deactivated/Dismissed`.

### Editor assistant

`Hephaestus → Utilities → ViewsCreatorAssistant` scaffolds the folder structure, the View/Model/Controller scripts and
an assembly definition under `Assets/<ProductName>/UI/<WidgetName>/`, and registers the new enum key in
`WidgetsLibraryConstants`.

## License

See [LICENSE.md](LICENSE.md).
