namespace MiniProjet_M2.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Age { get; set; }
    
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Age: {Age}";
    }
}