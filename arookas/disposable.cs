using System;

namespace arookas {
	public abstract class aDisposable : IDisposable {
		bool mDisposed;

		public bool IsDisposed {
			get { return mDisposed; }
		}

		protected aDisposable() { }
		~aDisposable() {
			if (!IsDisposed) {
				mDisposed |= Dispose(true);
			}
		}

		public void Dispose() {
			if (!mDisposed) {
				mDisposed |= Dispose(false);
			}
		}
		protected abstract bool Dispose(bool destructor);
	}
}
