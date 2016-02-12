using System;
using System.Linq;

namespace arookas.Collections {
	class aPool<T> : IDisposable
		where T : class, new() {
		T[] mItems;
		int mAlloc, mFree, mLeft, mCount;
		bool mDisposed;

		public int Count {
			get { return mCount; }
		}

		public aPool(int count) {
			aError.CheckMin(count, "count", 0);
			mCount = count;
			mItems = aCollection.Initialize(mCount, () => new T());
		}
		~aPool() {
			Dispose();
		}

		public T Alloc() {
			if (mLeft == 0) {
				return null;
			}
			var ret = mItems[mAlloc++];
			--mLeft;
			if (mAlloc == mCount) {
				mAlloc = 0;
			}
			return ret;
		}
		public bool Free(T item) {
			if (item == null || mLeft == mCount) {
				return false;
			}
			mItems[mFree++] = item;
			++mLeft;
			if (mFree == mCount) {
				mFree = 0;
			}
			return true;
		}

		public void Dispose() {
			if (!mDisposed && typeof(T) is IDisposable) {
				foreach (var item in mItems.OfType<IDisposable>()) {
					item.Dispose();
				}
			}
			mDisposed = true;
		}
	}
}
