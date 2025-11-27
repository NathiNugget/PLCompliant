using PLCompliant.EventArguments;
using PLCompliant.Events;
using PLCompliant.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace PLCompliantTests;

class TestUpdateThreadContext : UpdateThreadContext
{

}
class TestUpdateThreadContext2 : UpdateThreadContext
{

}

class TestRaisedEventArgs : RaisedEventArgs
{

}

class TestRaisedEventArgs2 : RaisedEventArgs
{

}

[ExcludeFromCodeCoverage]
[TestClass]
public class EventUtilitiesTests
{
    [TestMethod]
    public void ValidateContextFailNull()
    {
        UpdateThreadContext? context = null;
        Assert.ThrowsException<ArgumentNullException>(() => { EventUtilities.ValidateContext<TestUpdateThreadContext, UpdateThreadContext>(context); });
    }
    [TestMethod]
    public void ValidateContextFailInvalidCast()
    {
        UpdateThreadContext? context = new TestUpdateThreadContext();
        // tries to cast to TestUpdateThreadContext2 instead
        Assert.ThrowsException<InvalidCastException>(() => { EventUtilities.ValidateContext<TestUpdateThreadContext2, UpdateThreadContext>(context); });
    }
    [TestMethod]
    public void ValidateContextSuceed()
    {
        UpdateThreadContext context = new TestUpdateThreadContext();
        var result = EventUtilities.ValidateContext<TestUpdateThreadContext, UpdateThreadContext>(context);

        Assert.IsNotNull(result);
    }




    public void ValidateContextAndArgsFailContextNull()
    {
        UpdateThreadContext? context = null;
        RaisedEventArgs args = new TestRaisedEventArgs();
        Assert.ThrowsException<ArgumentNullException>(() => { EventUtilities.ValidateContextAndArgs<TestUpdateThreadContext, TestRaisedEventArgs, UpdateThreadContext, RaisedEventArgs>(context, args); });
    }
    public void ValidateContextAndArgsFailArgNull()
    {
        UpdateThreadContext? context = new TestUpdateThreadContext();
        RaisedEventArgs args = null;
        Assert.ThrowsException<ArgumentNullException>(() => { EventUtilities.ValidateContextAndArgs<TestUpdateThreadContext, TestRaisedEventArgs, UpdateThreadContext, RaisedEventArgs>(context, args); });
    }
    public void ValidateContextAndArgsInvalidCastArgs()
    {
        UpdateThreadContext? context = new TestUpdateThreadContext();
        RaisedEventArgs args = new TestRaisedEventArgs();
        //args attempted to be cast to wrong type
        Assert.ThrowsException<InvalidCastException>(() => { EventUtilities.ValidateContextAndArgs<TestUpdateThreadContext, TestRaisedEventArgs2, UpdateThreadContext, RaisedEventArgs>(context, args); });
    }
    public void ValidateContextAndArgsInvalidCastContext()
    {
        UpdateThreadContext? context = new TestUpdateThreadContext();
        RaisedEventArgs args = new TestRaisedEventArgs();
        //args attempted to be cast to wrong type
        Assert.ThrowsException<InvalidCastException>(() => { EventUtilities.ValidateContextAndArgs<TestUpdateThreadContext2, TestRaisedEventArgs, UpdateThreadContext, RaisedEventArgs>(context, args); });
    }



    [TestMethod]
    public void ValidateContextAndArgsSuceed()
    {
        UpdateThreadContext context = new TestUpdateThreadContext();
        RaisedEventArgs args = new TestRaisedEventArgs();
        var result = EventUtilities.ValidateContextAndArgs<TestUpdateThreadContext, TestRaisedEventArgs, UpdateThreadContext, RaisedEventArgs>(context, args);

        Assert.IsNotNull(result);
    }






}
