using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestEnum
{
    haha,
    hehe,
}
public class ReverserClass : IComparer<string>
{
   // Call CaseInsensitiveComparer.Compare with the parameters reversed.

   //int IComparer.Compare(System.Object x, System.Object y)
   // {
   //     return ((new CaseInsensitiveComparer()).Compare(y, x));
   // }
    public int Compare(object x, object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(y, x));
    }

    public int Compare(string x, string y)
    {
        return ((new CaseInsensitiveComparer()).Compare(y, x));
    }
}

public class ReverserClassPerson : IComparer<Person>
{
    public int Compare(Person x, Person y)
    {
        return ((new CaseInsensitiveComparer()).Compare(y, x));
    }
}
public class ListTest : MonoBehaviour
{
   
    public TestEnum testEnum = TestEnum.haha;
    private List<int> intList = new List<int>();
    private Dictionary<int, int> intDic = new Dictionary<int, int>();
    // Start is called before the first frame update
    void Start()
    {

       
        
        intList.Add(2);
        intList.Add(9);
        intList.Add(15);
        intList.Add(1);
        intList.Add(7);
        intList.Add(4);
        //intList.Sort();
        Comparison<int> comparison = new Comparison<int>((int a,int b) => {
            return b.CompareTo(a);
        });

        //intList.Sort((a,b)=> {
        //    return b.CompareTo(a);
        //});
        
        intList.Sort(new Compar());
        //foreach (var item in intList)
        //{
        //    Debug.Log(item);
        //}
        intDic.Add(0, 0);
        intDic.Add(3, 1);
        int[] intArray = new int[3] { 2,2,2};
        intDic.Keys.CopyTo(intArray,1);
        foreach (var item in intArray)
        {
            Debug.Log(item);
        }
        //intDic.Remove(0);
        //intDic.Add(0,0);
        //foreach (var item in intDic.Keys)
        //{
        //    Debug.Log(item);
        //}

        //=============================
        //=========Class============
        //=============================


        var personList = new List<Person>();
        var zhangSan = new Person("张三", 20);

        personList.Add(zhangSan);
        personList.Add(new Person("李四",80));
        personList.Add(new Person("王二麻子",10));
        personList.Add(new Person("江峰",70));
        personList.Add(new Person("阿腾",100));

        
        personList.Sort(zhangSan) ;
        
        foreach (var item in personList)
        {
            //Debug.Log(item.name);
        }

        var sixClass = new SixClass();
        var temp = sixClass.People;
        sixClass.People.RemoveRange(1,3);
        //temp.Clear();
        //temp.RemoveAt(0);
        
        sixClass.ConsleWrite();


        return;
        // Initialize a string array.
        string[] words = { "The", "quick", "brown", "fox", "jumps", "over",
                         "the", "lazy", "dog" };
        var strList = new List<string>();
        strList.AddRange(words);
        // Display the array values.
        Console.WriteLine("The array initially contains the following values:");
        PrintIndexAndValues(strList);

        // Sort the array values using the default comparer.
        Array.Sort(words);
        strList.Sort();
        Console.WriteLine("After sorting with the default comparer:");
        PrintIndexAndValues(strList);

        // Sort the array values using the reverse case-insensitive comparer.
        Array.Sort(words, new ReverserClass());
        strList.Sort(new ReverserClass());
        Console.WriteLine("After sorting with the reverse case-insensitive comparer:");
        PrintIndexAndValues(strList);
    }

    public void PrintIndexAndValues(IEnumerable list)
    {
        int i = 0;
        foreach (var item in list)
            print($"   [{i++}]:  {item}");

        Console.WriteLine();
    }

    public class Compar : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }
    }


    public int Sort(int a,int b)
    {
        return a.CompareTo(b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class SixClass {
    public int Count { get; private set; }
    public List<Person> People { get; private set; }
    public SixClass()
    {
        People = new List<Person>();
        People.Add(new Person("李四", 80));
        People.Add(new Person("王二麻子", 10));
        People.Add(new Person("江峰", 70));
        People.Add(new Person("阿腾", 100));
        Count = People.Count;
    }

    public List<Person> GetPerson()
    {
        return People;
    }

    public void ConsleWrite()
    {
        foreach (var item in People)
        {
            Debug.Log(item.name);
        }
    }
}

public class Person : IComparable<Person> ,IComparer<Person>
{
    public string name;
    public int age;

    public Person()
    {
    }

    public Person(string name,int age)
    {
        this.name = name;
        this.age = age;
    }

    public int Compare(Person x, Person y)
    {
        return y.CompareTo(x);
    }

    public int CompareTo(object obj)
    {
        if (this.age > (int)obj)
            return 1;
        else if (this.age == (int)obj)
            return 0;
        else
            return -1;
    }

    public int CompareTo(Person other)
    {
        if (this.age < other.age)
            return 1;
        else if (this.age == other.age)
            return 0;
        else
            return -1;
    }
}