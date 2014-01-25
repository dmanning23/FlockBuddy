using System.Collections.Generic;

namespace FlockBuddy
{
	/// <summary>
	/// Template class to help calculate the average value of a history of values. 
	/// This can only be used with types that have a 'zero' and that have the += and / operators overloaded.
	/// Example: Used to smooth frame rate calculations.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Smoother<T>
	{
		#region Members

		/// <summary>
		/// this holds the history
		/// </summary>
		private Queue<T> History { get; set; }

		/// <summary>
		/// The max smaple size we want
		/// </summary>
		public int MaxSize { get; set; }

		/// <summary>
		/// an example of the 'zero' value of the type to be smoothed. 
		/// This would be something like Vector2D(0,0)
		/// </summary>
		private T ZeroValue;

		#endregion //Members

		#region Methods

		//to instantiate a Smoother pass it the number of samples you want
		//to use in the smoothing, and an exampe of a 'zero' type
		public Smoother(int SampleSize, T ZeroValue)
		{
			m_History = new Queue<T>();
			MaxSize = SampleSize;
			m_ZeroValue = ZeroValue;
		}

		/// <summary>
		/// each time you want to get a new average, feed it the most recent value
		/// and this method will return an average over the last SampleSize updates
		/// </summary>
		/// <param name="MostRecentValue"></param>
		/// <returns></returns>
		public T Update(T MostRecentValue)
		{
			//add the new value to the beginning
			History.Enqueue(MostRecentValue);

			//pop enough items off to stay in the correct size
			while (History.Count > MaxSize)
			{
				History.Dequeue();
			}

			//now to calculate the average of the history list
			T sum = m_ZeroValue;

			foreach (var iter in History)
			{
				sum += iter;
			}

			return sum / (float)History.size();
		}

		#endregion //Methods
	}
}