using System;
using System.IO;
using System.Xml.Serialization;

namespace ViewportGame.Util.XML;

public class XmlManager<T>
{
    public Type Type;

    public T Load(string path)
    {
        T instance;
        using (TextReader reader = new StreamReader(path))
        {
            XmlSerializer xml = new(Type);
            instance = (T)xml.Deserialize(reader);
        }

        return instance;
    }

    public void Save(string path, object obj)
    {
        using (TextWriter writer = new StreamWriter(path))
        {
            XmlSerializer xml = new(Type);
            xml.Serialize(writer, obj);
        }
    }
}