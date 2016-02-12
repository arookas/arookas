using arookas.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arookas {
	public sealed class aCommandLine : IEnumerable<aCommandLineParameter> {
		aCommandLineParameter[] mParams;

		public int Count {
			get { return mParams.Length; }
		}

		public aCommandLineParameter this[int index] {
			get { return mParams[index]; }
		}

		public aCommandLine(string[] args)
			: this(args, '-') { }
		public aCommandLine(string[] args, char delimiter) {
			aError.CheckNull(args, "args");
			aError.Check<ArgumentException>(args.All(i => i != null), "The specified argument collection contains at least one null element.", "args");
			var parameters = new List<aCommandLineParameter>(10);
			var paramArgs = new List<string>(10);
			string param = null;
			foreach (var arg in args) {
				if (arg[0] == delimiter) {
					if (param != null) {
						parameters.Add(new aCommandLineParameter(param, paramArgs.ToArray()));
					}
					param = arg;
					paramArgs.Clear();
				}
				else {
					if (param != null) {
						paramArgs.Add(arg);
					}
					else {
						parameters.Add(new aCommandLineParameter(arg, new string[0]));
					}
				}
			}
			if (param != null) {
				parameters.Add(new aCommandLineParameter(param, paramArgs.ToArray()));
			}
			mParams = parameters.ToArray();
		}

		public IEnumerator<aCommandLineParameter> GetEnumerator() {
			return mParams.GetArrayEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}

	public sealed class aCommandLineParameter : IEnumerable<string> {
		string mName;
		string[] mArgs;

		public string Name {
			get { return mName; }
		}
		public int Count {
			get { return mArgs.Length; }
		}

		public string this[int index] {
			get { return mArgs[index]; }
		}

		internal aCommandLineParameter(string name, string[] args) {
			mName = name;
			mArgs = args;
		}

		public IEnumerator<string> GetEnumerator() {
			return mArgs.GetArrayEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public override string ToString() {
			if (mArgs.Length > 0) {
				var sb = new StringBuilder();
				sb.Append(Name);
				foreach (var arg in this) {
					if (arg.Any(c => Char.IsWhiteSpace(c))) {
						sb.AppendFormat(" \"{0}\"", arg);
					}
					else {
						sb.AppendFormat(" {0}", arg);
					}
				}
				return sb.ToString();
			}
			return Name;
		}

		// for use in switch statements
		public static implicit operator string(aCommandLineParameter parameter) {
			return parameter.Name;
		}
	}
}
