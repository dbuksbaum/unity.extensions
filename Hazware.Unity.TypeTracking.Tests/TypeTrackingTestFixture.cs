using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Hazware.Unity.TypeTracking.Tests
{
  [TestFixture]
  public class TypeTrackingTestFixture
  {
    #region Fields
    private IUnityContainer _container = null;
    #endregion

    #region Setup & Term
    /// <summary>
    /// Inits each test by creating a new container and registering the <see cref="TypeTrackingExtension"/>.
    /// </summary>
    [SetUp]
    public void Init()
    {
      _container = new UnityContainer();
      _container.AddNewExtension<TypeTrackingExtension>();
    }
    /// <summary>
    /// Tears down each test by setting the container to null.
    /// </summary>
    [TearDown]
    public void Term()
    {
      _container = null;
    }
    #endregion

    #region TryResolveUnknown
    [Test]
    public void TryResolveOfUnknownType()
    {
      ITest obj = _container.Configure<TypeTrackingExtension>().TryResolve<ITest>();
      Assert.IsNull(obj);
    }
    [Test]
    public void TryResolveOfUnknownTypeByName()
    {
      ITest obj = _container.Configure<TypeTrackingExtension>().TryResolve<ITest>("name");
      Assert.IsNull(obj);
    }
    #endregion

    #region TryResolve
    [Test]
    public void TryResolveOfRegisteredType()
    {
      _container.RegisterType<ITest, TestClass>();
      Assert.IsNotNull(_container.Resolve<ITest>());
      ITest obj = _container.Configure<TypeTrackingExtension>().TryResolve<ITest>();
      Assert.IsNotNull(obj);
    }
    [Test]
    public void TryResolveOfRegisteredTypeByName()
    {
      _container.RegisterType<ITest, TestClass>("name");
      ITest obj = _container.Configure<TypeTrackingExtension>().TryResolve<ITest>("name");
      Assert.IsNotNull(obj);
    }
    #endregion

    #region TryResolve With Defaults
    [Test]
    public void TryResolveOfUnknownTypeWithDefault()
    {
      TestClass src = new TestClass();
      src.Data = "hello world";
      ITest obj = _container.Configure<TypeTrackingExtension>().TryResolve<ITest>(src);
      Assert.IsNotNull(obj);
      Assert.AreSame(src, obj);
    }
    [Test]
    public void TryResolveOfUnknownTypeWithDefaultByName()
    {
      TestClass src = new TestClass();
      src.Data = "hello world";
      ITest obj = _container.Configure<TypeTrackingExtension>().TryResolve<ITest>("name", src);
      Assert.IsNotNull(obj);
      Assert.AreSame(src, obj);
    }
    #endregion

    #region CanResolve
    [Test]
    public void CanResolveType()
    {
      Assert.IsFalse(_container.Configure<TypeTrackingExtension>().CanResolve<ITest>());
      _container.RegisterType<ITest, TestClass>();
      Assert.IsTrue(_container.Configure<TypeTrackingExtension>().CanResolve<ITest>());
    }
    [Test]
    public void CanResolveByName()
    {
      Assert.IsFalse(_container.Configure<TypeTrackingExtension>().CanResolve<ITest>("name"));
      _container.RegisterType<ITest, TestClass>("name");
      Assert.IsTrue(_container.Configure<TypeTrackingExtension>().CanResolve<ITest>("name"));
    }
    #endregion

    #region ImplicitDefaultResolveAll
    [Test]
    public void ImplicitDefaultResolveAllDefaultWithDefaultTypeOnly()
    {
      _container.RegisterType<ITest, TestClass>();
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>())
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(1, count);
    }
    [Test]
    public void ImplicitDefaultResolveAllDefaultWithNamedTypeOnly()
    {
      _container.RegisterType<ITest, TestClass>("name");
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>())
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(1, count);
    }
    [Test]
    public void ImplicitDefaultResolveAllDefaultWithTwoTypes()
    {
      _container.RegisterType<ITest, TestClass>();
      _container.RegisterType<ITest, AnotherTestClass>("name");
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>())
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(2, count);
    }
    [Test]
    public void ImplicitDefaultResolveAllDefaultWithNoTypes()
    {
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>())
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(0, count);
    }
    #endregion

    #region ExplicitDefaultResolveAll
    [Test]
    public void ExplicitDefaultResolveAllDefaultWithDefaultTypeOnly()
    {
      _container.RegisterType<ITest, TestClass>();

      //  test with default
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(true))
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(1, count);

      //  test without default
      count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(false))
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(0, count);
    }
    [Test]
    public void ExplicitDefaultResolveAllDefaultWithNamedTypeOnly()
    {
      _container.RegisterType<ITest, TestClass>("name");
      //  test with default
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(true))
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(1, count);

      //  test without default
      count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(false))
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(1, count);
    }
    [Test]
    public void ExplicitDefaultResolveAllDefaultWithTwoTypes()
    {
      _container.RegisterType<ITest, TestClass>();
      _container.RegisterType<ITest, AnotherTestClass>("name");
      //  test with default
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(true))
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(2, count);

      //  test without default
      count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(false))
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(1, count);
    }
    [Test]
    public void ExplicitDefaultResolveAllDefaultWithNoTypes()
    { //  test with default
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(true))
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(0, count);

      //  test with default
      count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAll<ITest>(false))
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(0, count);
    }
    #endregion

    #region ImplicitDefaultResolveAllToArray
    [Test]
    public void ImplicitDefaultResolveAllToArrayDefaultWithDefaultTypeOnly()
    {
      _container.RegisterType<ITest, TestClass>();
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>())
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(1, count);
    }
    [Test]
    public void ImplicitDefaultResolveAllToArrayDefaultWithNamedTypeOnly()
    {
      _container.RegisterType<ITest, TestClass>("name");
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>())
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(1, count);
    }
    [Test]
    public void ImplicitDefaultResolveAllToArrayDefaultWithTwoTypes()
    {
      _container.RegisterType<ITest, TestClass>();
      _container.RegisterType<ITest, AnotherTestClass>("name");
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>())
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(2, count);
    }
    [Test]
    public void ImplicitDefaultResolveAllToArrayDefaultWithNoTypes()
    {
      int count = 0;
      foreach (var item in _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>())
      {
        Assert.IsNotNull(item);
        ++count;
      }
      Assert.AreEqual(0, count);
    }
    #endregion

    #region ExplicitDefaultResolveAllToArray
    [Test]
    public void ExplicitDefaultResolveAllToArrayDefaultWithDefaultTypeOnly()
    {
      _container.RegisterType<ITest, TestClass>();

      //  test with default
      var objs = _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(true);
      Assert.IsNotNull(objs);
      Assert.AreEqual(1, objs.Length);

      //  test without default
      objs = _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(false);
      Assert.IsNotNull(objs);
      Assert.AreEqual(0, objs.Length);
    }
    [Test]
    public void ExplicitDefaultResolveAllToArrayDefaultWithNamedTypeOnly()
    {
      _container.RegisterType<ITest, TestClass>("name");

      //  test with default
      var objs = _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(true);
      Assert.IsNotNull(objs);
      Assert.AreEqual(1, objs.Length);

      //  test without default
      objs = _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(false);
      Assert.IsNotNull(objs);
      Assert.AreEqual(1, objs.Length);
    }
    [Test]
    public void ExplicitDefaultResolveAllToArrayDefaultWithTwoTypes()
    {
      _container.RegisterType<ITest, TestClass>();
      _container.RegisterType<ITest, AnotherTestClass>("name");

      //  test with default
      var objs = _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(true);
      Assert.IsNotNull(objs);
      Assert.AreEqual(2, objs.Length);

      //  test without default
      objs = _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(false);
      Assert.IsNotNull(objs);
      Assert.AreEqual(1, objs.Length);
    }
    [Test]
    public void ExplicitDefaultResolveAllToArrayDefaultWithNoTypes()
    {
      //  test with default
      var objs = _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(true);
      Assert.IsNotNull(objs);
      Assert.AreEqual(0, objs.Length);

      //  test without default
      objs = _container.Configure<TypeTrackingExtension>().ResolveAllToArray<ITest>(false);
      Assert.IsNotNull(objs);
      Assert.AreEqual(0, objs.Length);
    }
    #endregion
  }
}
