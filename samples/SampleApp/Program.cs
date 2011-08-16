using System;
using System.Linq;
using Hazware.Unity.TypeTracking;
using Microsoft.Practices.Unity;

namespace SampleApp
{
  #region Sample Classes and Interfaces
  interface IFoo
  {
    void Test();
  }
  interface IBar
  {
    void Test();
  }
  class BaseFoo : IFoo
  {
    #region IFoo Members
    public void Test()
    {
      Console.WriteLine(GetType().Name + " Test()");
    }
    #endregion
  }
  class AFoo : BaseFoo
  {
  }
  class AnotherFoo : BaseFoo
  {
  }
  class ABar : IBar
  {
    #region IBar Members
    public void Test()
    {
      throw new NotImplementedException();
    }
    #endregion
  }
  #endregion

  class Program
  {
    static void Main(string[] args)
    {
      IUnityContainer container = new UnityContainer();
      container.AddNewExtension<TypeTrackingExtension>();

      container.RegisterType<IFoo, AFoo>();
      container.RegisterType<IFoo, AnotherFoo>("Named");

      Console.WriteLine("CanResolve<IFoo>() == {0}", container.CanResolve<IFoo>());
      Console.WriteLine("CanResolve<IFoo>(\"Named\") == {0}", container.CanResolve<IFoo>("Named"));
      Console.WriteLine("CanResolve<IBar>() == {0}", container.CanResolve<IBar>());

      Console.WriteLine("TryResolve<IFoo>() == null ==> {0}", container.TryResolve<IFoo>() == null);
      Console.WriteLine("TryResolve<IFoo>(\"Named\") == null ==> {0}", container.TryResolve<IFoo>("Named") == null);
      Console.WriteLine("TryResolve<IBar>() == null ==> {0}", container.TryResolve<IBar>() == null);
      
      Console.WriteLine("TryResolve<IBar>(new ABar()) == null ==> {0}", container.TryResolve<IBar>(new ABar()) == null);
      Console.WriteLine("TryResolve<IBar>(\"Named\", new ABar()) == null ==> {0}", container.TryResolve<IBar>("Named", new ABar()) == null);

      Console.WriteLine("ResolveAllToEnumerable<IFoo>().Count() == {0}", container.ResolveAllToEnumerable<IFoo>().Count());
      Console.WriteLine("ResolveAllToEnumerable<IFoo>(false).Count() == {0}", container.ResolveAllToEnumerable<IFoo>(false).Count());
      Console.WriteLine("ResolveAllToEnumerable<IBar>().Count() == {0}", container.ResolveAllToEnumerable<IBar>().Count());

      Console.WriteLine("ResolveAllToArray<IFoo>().Length == {0}", container.ResolveAllToArray<IFoo>().Length);
      Console.WriteLine("ResolveAllToArray<IFoo>(false).Length == {0}", container.ResolveAllToArray<IFoo>(false).Length);
      Console.WriteLine("ResolveAllToArray<IBar>().Length == {0}", container.ResolveAllToArray<IBar>().Length);

      Console.ReadLine();
    }
  }
}
