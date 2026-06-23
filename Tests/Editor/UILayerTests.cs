using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace WTFGames.Hephaestus.UISystem.Tests
{
    public class UILayerTests
    {
        private readonly List<GameObject> _spawned = new List<GameObject>();
        private UILayer _layer;

        [SetUp]
        public void SetUp()
        {
            _layer = NewGameObject("TestUILayer").AddComponent<UILayer>();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var go in _spawned)
            {
                if (go != null)
                {
                    Object.DestroyImmediate(go);
                }
            }

            _spawned.Clear();
        }

        [Test]
        public void RegisterWidget_MakesTypeExist()
        {
            _layer.RegisterWidget(TestWidgetType.Menu, NewWidget());

            Assert.IsTrue(_layer.IsWidgetTypeAlreadyExists(TestWidgetType.Menu));
            Assert.AreEqual(1, _layer.GetWidgetsCount());
        }

        [Test]
        public void RegisterWidget_ParentsWidgetUnderLayer()
        {
            var widget = NewWidget();

            _layer.RegisterWidget(TestWidgetType.Menu, widget);

            Assert.AreSame(_layer.transform, widget.Transform.parent);
        }

        [Test]
        public void IsWidgetTypeAlreadyExists_IsFalse_ForUnknownType()
        {
            Assert.IsFalse(_layer.IsWidgetTypeAlreadyExists(TestWidgetType.Hud));
        }

        [Test]
        public void GetWidgetByType_ReturnsNull_WhenMissing()
        {
            Assert.IsNull(_layer.GetWidgetByType(TestWidgetType.Hud));
        }

        [Test]
        public void AllowDuplicates_StoresMultipleWidgetsOfSameType()
        {
            // Regression: previously a duplicate key threw ArgumentException (#1).
            _layer.RegisterWidget(TestWidgetType.Popup, NewWidget());
            _layer.RegisterWidget(TestWidgetType.Popup, NewWidget());

            Assert.AreEqual(2, _layer.GetWidgetsCount());
            Assert.AreEqual(2, _layer.GetAllWidgetsInLayer().Count);
        }

        [Test]
        public void GetWidgetByType_ReturnsMostRecentlyRegistered()
        {
            var first = NewWidget();
            var second = NewWidget();
            _layer.RegisterWidget(TestWidgetType.Popup, first);
            _layer.RegisterWidget(TestWidgetType.Popup, second);

            Assert.AreSame(second, _layer.GetWidgetByType(TestWidgetType.Popup));
        }

        [Test]
        public void GetWidgetsCount_SumsAcrossTypes()
        {
            _layer.RegisterWidget(TestWidgetType.Menu, NewWidget());
            _layer.RegisterWidget(TestWidgetType.Popup, NewWidget());
            _layer.RegisterWidget(TestWidgetType.Popup, NewWidget());

            Assert.AreEqual(3, _layer.GetWidgetsCount());
        }

        [Test]
        public void DismissingWidget_RemovesItFromLayer()
        {
            var widget = NewWidget();
            _layer.RegisterWidget(TestWidgetType.Menu, widget);

            widget.Dismiss(); // raises OnDismissed, which UILayer listens to

            Assert.IsFalse(_layer.IsWidgetTypeAlreadyExists(TestWidgetType.Menu));
            Assert.AreEqual(0, _layer.GetWidgetsCount());
        }

        [Test]
        public void DismissingOneDuplicate_KeepsTypePresent()
        {
            var first = NewWidget();
            var second = NewWidget();
            _layer.RegisterWidget(TestWidgetType.Popup, first);
            _layer.RegisterWidget(TestWidgetType.Popup, second);

            first.Dismiss();

            Assert.IsTrue(_layer.IsWidgetTypeAlreadyExists(TestWidgetType.Popup));
            Assert.AreEqual(1, _layer.GetWidgetsCount());
            Assert.AreSame(second, _layer.GetWidgetByType(TestWidgetType.Popup));
        }

        [Test]
        public void GetAllWidgetsInLayer_ReturnsSnapshot_SafeToDismissWhileIterating()
        {
            _layer.RegisterWidget(TestWidgetType.Menu, NewWidget());
            _layer.RegisterWidget(TestWidgetType.Popup, NewWidget());

            // Mirrors UIManagerHandler.DismissWidgetsInLayer: iterate the snapshot and dismiss.
            foreach (var widget in _layer.GetAllWidgetsInLayer())
            {
                widget.Dismiss();
            }

            Assert.AreEqual(0, _layer.GetWidgetsCount());
        }

        private TestWidget NewWidget()
        {
            var widget = new TestWidget();
            _spawned.Add(widget.GameObject);
            return widget;
        }

        private GameObject NewGameObject(string name)
        {
            var go = new GameObject(name);
            _spawned.Add(go);
            return go;
        }
    }
}
