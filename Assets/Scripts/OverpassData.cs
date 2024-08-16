using System;
using System.Collections.Generic;

[Serializable]
public class OverpassData
{
    public List<Element> elements;
}

[Serializable]
public class Element
{
    public string type;
    public long id;
    public List<long> nodes;
    public Dictionary<string, string> tags;
}

[Serializable]
public class Node
{
    public string type;
    public long id;
    public double lat;
    public double lon;
}
