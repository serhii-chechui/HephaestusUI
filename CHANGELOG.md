## <small>2.3.1 (2026-06-23)</small>

* test(ui): add EditMode tests for UILayer and WidgetsLibrary ([f71b293](https://github.com/serhii-chechui/HephaestusUI/commit/f71b293))
* fix(tests): make TestWidget a plain class so it works in the Editor-only test assembly ([fa2615a](https://github.com/serhii-chechui/HephaestusUI/commit/fa2615a))



## <small>2.3.0 (2026-06-23)</small>

* fix(ui): support allowDuplicates by storing widgets per type in lists ([26e2404](https://github.com/serhii-chechui/HephaestusUI/commit/26e2404))
* fix(ui): guard against NRE when sharedInstance has no UI camera ([26e2404](https://github.com/serhii-chechui/HephaestusUI/commit/26e2404))
* fix(ui): DismissWidgetsInLayer now dismisses all widgets in the layer ([26e2404](https://github.com/serhii-chechui/HephaestusUI/commit/26e2404))
* fix(ui): stop competing fade coroutines and activate before animated fade-in ([26e2404](https://github.com/serhii-chechui/HephaestusUI/commit/26e2404))
* refactor(ui): make UIManagerHandler DI-owned and free resources on dispose ([767ea25](https://github.com/serhii-chechui/HephaestusUI/commit/767ea25))
* fix(ui): add guards for missing library entries, controllers and UI layer ([2f9c4d8](https://github.com/serhii-chechui/HephaestusUI/commit/2f9c4d8))
* chore(ui): remove non-functional preloader API, cache CanvasGroup, configurable fade duration ([eb1f221](https://github.com/serhii-chechui/HephaestusUI/commit/eb1f221))
* docs: add full README (usage, setup, widget authoring) ([eb1f221](https://github.com/serhii-chechui/HephaestusUI/commit/eb1f221))



## <small>2.0.1 (2025-01-27)</small>

* 2.0.1 ([2c29e4f](https://github.com/serhii-chechui/HephaestusUI/commit/2c29e4f))



## <small>2.0.2 (2025-01-27)</small>

* 2.0.1 ([975fcb4](https://github.com/serhii-chechui/HephaestusUI/commit/975fcb4))
* 2.0.2 ([29f9367](https://github.com/serhii-chechui/HephaestusUI/commit/29f9367))
* fix(editor): fixes the bug with the order of scenes folder creation ([371876a](https://github.com/serhii-chechui/HephaestusUI/commit/371876a))



## <small>2.0.1 (2025-01-27)</small>

* 2.0.1 ([2c29e4f](https://github.com/serhii-chechui/HephaestusUI/commit/2c29e4f))
* docs(changelog): updates changelog ([b6ead10](https://github.com/serhii-chechui/HephaestusUI/commit/b6ead10))
* refactor(editor): improves widget assistance workflow ([d1706d4](https://github.com/serhii-chechui/HephaestusUI/commit/d1706d4))



## 2.0.0 (2024-10-08)

* 2.0.0 ([033b139](https://github.com/serhii-chechui/HephaestusUI/commit/033b139))
* refactor: small cleanup, made Activate, Deactivate and Dismiss virtual for basewidget ([b385eda](https://github.com/serhii-chechui/HephaestusUI/commit/b385eda))



## <small>1.1.1 (2023-08-31)</small>

* [Changed] Updated .gitignore ([d850f82](https://github.com/serhii-chechui/HephaestusUI/commit/d850f82))
* [Changed] Updated CHANGELOG.md and package.json ([386fa1a](https://github.com/serhii-chechui/HephaestusUI/commit/386fa1a))
* 1.1.1 ([894b2a5](https://github.com/serhii-chechui/HephaestusUI/commit/894b2a5))



## 1.1.0 (2023-05-21)

* [Added] Added ability use WidgetsLibraryConstants as a lis of keys for work with the UI wigdets. ([f04d8af](https://github.com/serhii-chechui/HephaestusUI/commit/f04d8af))
* [Changed] Updated the publish config. ([40c499c](https://github.com/serhii-chechui/HephaestusUI/commit/40c499c))
* 1.1.0 ([d53bb50](https://github.com/serhii-chechui/HephaestusUI/commit/d53bb50))



## <small>1.0.2 (2022-12-15)</small>

* 1.0.2 ([fba49df](https://github.com/serhii-chechui/HephaestusUI/commit/fba49df))
* Added scripts define symbols ([c623f48](https://github.com/serhii-chechui/HephaestusUI/commit/c623f48))



## <small>1.0.1 (2022-12-15)</small>

* 1.0.1 ([6c8f34e](https://github.com/serhii-chechui/HephaestusUI/commit/6c8f34e))
* Renamed assemblies. Used version defines. ([902cb6e](https://github.com/serhii-chechui/HephaestusUI/commit/902cb6e))



## 1.0.0 (2022-09-08)

* 1.0.0 ([d0e6a2b](https://github.com/serhii-chechui/HephaestusUI/commit/d0e6a2b))
* Reworked and changed approach for UIManager initialization. ([e6a665d](https://github.com/serhii-chechui/HephaestusUI/commit/e6a665d))



## <small>0.0.5 (2021-08-19)</small>

* Updated Action for Verdaccio ([81ec67e](https://github.com/serhii-chechui/HephaestusUI/commit/81ec67e))
* Updated version to deploy at Verdaccio ([0e6bd36](https://github.com/serhii-chechui/HephaestusUI/commit/0e6bd36))



## <small>0.0.4 (2021-04-12)</small>

* Added universal render pipeline into dependencies. ([9c77f44](https://github.com/serhii-chechui/HephaestusUI/commit/9c77f44))
* Changed Hephaestus Core dependency version. ([defa813](https://github.com/serhii-chechui/HephaestusUI/commit/defa813))
* Create main.yml ([1d32dc2](https://github.com/serhii-chechui/HephaestusUI/commit/1d32dc2))
* Fixed issue with ui manager config editor. ([19c1de4](https://github.com/serhii-chechui/HephaestusUI/commit/19c1de4))
* Set camera render type in a properly way. ([3e917a9](https://github.com/serhii-chechui/HephaestusUI/commit/3e917a9))
* Set properly version and repository link. ([23f0698](https://github.com/serhii-chechui/HephaestusUI/commit/23f0698))
* Updated Core version to 0.0.4 ([c94fcbb](https://github.com/serhii-chechui/HephaestusUI/commit/c94fcbb))



## <small>0.0.3 (2021-04-11)</small>

* Add files via upload ([5772188](https://github.com/serhii-chechui/HephaestusUI/commit/5772188))
* Added Zenject dependency ([09abef1](https://github.com/serhii-chechui/HephaestusUI/commit/09abef1))
* Fixed public modifier within the IWidget. ([6db5e91](https://github.com/serhii-chechui/HephaestusUI/commit/6db5e91))
* Initial commit ([f331562](https://github.com/serhii-chechui/HephaestusUI/commit/f331562))



