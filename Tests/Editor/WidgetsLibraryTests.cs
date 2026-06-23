using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace WTFGames.Hephaestus.UISystem.Tests
{
    public class WidgetsLibraryTests
    {
        private WidgetsLibrary _library;

        [SetUp]
        public void SetUp()
        {
            _library = ScriptableObject.CreateInstance<WidgetsLibrary>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_library);
        }

        [Test]
        public void GetLayerByType_ReturnsRegisteredLayer()
        {
            _library.widgetLinks.Add(new WidgetsLibraryData
            {
                WidgetType = (int)TestWidgetType.Popup,
                WidgetLayer = 3
            });

            Assert.AreEqual(3, _library.GetLayerByType(TestWidgetType.Popup));
        }

        [Test]
        public void GetLayerByType_ReturnsMinusOne_AndLogs_ForUnknownType()
        {
            // Regression: previously threw NRE on a missing entry (#7).
            LogAssert.Expect(LogType.Error, "WidgetsLibrary: no entry registered for widget type Menu.");

            Assert.AreEqual(-1, _library.GetLayerByType(TestWidgetType.Menu));
        }

        [Test]
        public void GetPrefabByType_ReturnsNull_AndLogs_WhenPrefabUnassigned()
        {
            _library.widgetLinks.Add(new WidgetsLibraryData
            {
                WidgetType = (int)TestWidgetType.Hud,
                WidgetPrefab = null
            });

            LogAssert.Expect(LogType.Error, "WidgetsLibrary: prefab for widget type Hud is not assigned.");

            Assert.IsNull(_library.GetPrefabByType(TestWidgetType.Hud));
        }

        [Test]
        public void GetPrefabByType_ReturnsNull_AndLogs_ForUnknownType()
        {
            LogAssert.Expect(LogType.Error, "WidgetsLibrary: no entry registered for widget type Popup.");

            Assert.IsNull(_library.GetPrefabByType(TestWidgetType.Popup));
        }
    }
}
