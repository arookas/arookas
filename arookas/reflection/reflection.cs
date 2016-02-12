using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace arookas.Reflection {
	public static class aReflection {
		public static T CreateInstance<T>(params object[] args) {
			aError.CheckNull(args, "args");
			try {
				return (T)Activator.CreateInstance(typeof(T), args);
			}
			catch (InvalidCastException) {
				return default(T);
			}
		}
		public static T CreateInstance<T>(Type type, params object[] args) {
			aError.CheckNull(type, "type");
			aError.CheckNull(args, "args");
			try {
				return (T)Activator.CreateInstance(type, args);
			}
			catch (InvalidCastException) {
				return default(T);
			}
		}

		public static Dictionary<TAttribute, Type> GetTypesWithAttribute<TAttribute>(this Assembly assembly)
			where TAttribute : Attribute {
			return GetTypesWithAttribute<TAttribute, Object>(assembly, true);
		}
		public static Dictionary<TAttribute, Type> GetTypesWithAttribute<TAttribute>(this Assembly assembly, bool publicOnly)
			where TAttribute : Attribute {
			return GetTypesWithAttribute<TAttribute, Object>(assembly, publicOnly, type => !type.IsAbstract);
		}
		public static Dictionary<TAttribute, Type> GetTypesWithAttribute<TAttribute>(this Assembly assembly, Predicate<Type> predicate)
			where TAttribute : Attribute {
			return GetTypesWithAttribute<TAttribute, Object>(assembly, true, predicate);
		}
		public static Dictionary<TAttribute, Type> GetTypesWithAttribute<TAttribute>(this Assembly assembly, bool publicOnly, Predicate<Type> predicate)
			where TAttribute : Attribute {
			return GetTypesWithAttribute<TAttribute, Object>(assembly, publicOnly, predicate);
		}
		public static Dictionary<TAttribute, Type> GetTypesWithAttribute<TAttribute, TBase>(this Assembly assembly)
			where TAttribute : Attribute {
			return GetTypesWithAttribute<TAttribute, TBase>(assembly, true);
		}
		public static Dictionary<TAttribute, Type> GetTypesWithAttribute<TAttribute, TBase>(this Assembly assembly, bool publicOnly)
			where TAttribute : Attribute {
			return GetTypesWithAttribute<TAttribute, TBase>(assembly, publicOnly, type => !type.IsAbstract);
		}
		public static Dictionary<TAttribute, Type> GetTypesWithAttribute<TAttribute, TBase>(this Assembly assembly, Predicate<Type> predicate)
			where TAttribute : Attribute {
			return GetTypesWithAttribute<TAttribute, TBase>(assembly, true, predicate);
		}
		public static Dictionary<TAttribute, Type> GetTypesWithAttribute<TAttribute, TBase>(this Assembly assembly, bool publicOnly, Predicate<Type> predicate)
			where TAttribute : Attribute {
			aError.CheckNull(assembly, "assembly");
			aError.CheckNull(predicate, "predicate");
			var foundTypes =
				from type in (publicOnly ? assembly.GetExportedTypes() : assembly.GetTypes())
				where typeof(TBase).IsAssignableFrom(type) // IsSubclassOf
				where predicate(type)
				let attributes = type.GetCustomAttributes(typeof(TAttribute), false)
				where attributes.Length > 0
				let attribute = (attributes[0] as TAttribute)
				select new KeyValuePair<TAttribute, Type>(attribute, type);
			return foundTypes.ToDictionary(k => k.Key, e => e.Value);
		}

		public static bool ImplementsInterface<TInterface>(this Type type)
			where TInterface : class {
			aError.CheckNull(type, "type");
			var interfaceType = typeof(TInterface);
			aError.Check<ArgumentException>(
				interfaceType.IsInterface,
				String.Format("The specified type '{0}' is not an interface.", interfaceType.Name),
				"type"
			);
			return (type.GetInterface(interfaceType.Name) != null);
		}
	}
}
