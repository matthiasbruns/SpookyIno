 using System.Reflection;
 using System.Linq;

public class ReflectionUtils {
    public static System.Type[] GetDerivedOfType<T>() where T : class {
        System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
        return (from System.Type type in types where type.IsSubclassOf(typeof(T)) select type).ToArray();
    }
}