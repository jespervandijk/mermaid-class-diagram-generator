using System.Reflection;
using FluentAssertions;
using MermaidClassDiagramGenerator;

namespace Test;

public class TestTypeExtensions
{
    private interface IVehicle
    {
    }
    private abstract class Vehicle
    {
    }
    
    private class Car : Vehicle, IVehicle
    {
    }
    
    private interface IPerson<T>
    {
    }
    private abstract class Person<T>
    {
    }
    private class Driver : Person<Driver>, IPerson<Driver>
    {
    }

    [Fact]
    public void InheritsFrom_ClassInheritsFromOtherClass_ReturnsTrue()
    {
        typeof(Car).InheritsFrom(typeof(Vehicle)).Should().BeTrue();
    }
    
    [Fact]
    public void InheritsFrom_ClassDoesntInheritFromOtherClass_ReturnsFalse()
    {
        typeof(Vehicle).InheritsFrom(typeof(Car)).Should().BeFalse();
    }
    
    [Fact]
    public void InheritsFromGenericType_ClassInheritsFromOtherClass_ReturnsTrue()
    {
        typeof(Driver).InheritsFromGenericType(typeof(Person<>)).Should().BeTrue();
    }
    
    [Fact]
    public void InheritsFromGenericType_ClassDoesntInheritFromOtherClass_ReturnsFalse()
    {
        typeof(Person<>).InheritsFrom(typeof(Driver)).Should().BeFalse();
    }

    [Fact]
    public void Implements_ClassImplementsInterface_ReturnsTrue()
    {
        typeof(Car).Implements(typeof(IVehicle)).Should().BeTrue();
    }
    
    [Fact]
    public void Implements_ClassDoesntImplementInterface_ReturnsFalse()
    {
        typeof(Driver).Implements(typeof(IVehicle)).Should().BeFalse();
    }

    [Fact]
    public void ImplementsGenericInterface_ClassImplementsInterface_ReturnsTrue()
    {
        typeof(Driver).ImplementsGenericInterface(typeof(IPerson<>)).Should().BeTrue();
    }
    
    [Fact]
    public void ImplementsGenericInterface_ClassImplementsInterface_ReturnsFalse()
    {
        typeof(Car).ImplementsGenericInterface(typeof(IPerson<>)).Should().BeFalse();
    }
}