// hate_coreml.cs
//
// This file was automatically generated and should not be edited.
//

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using CoreML;
using CoreVideo;
using Foundation;

namespace XamarinNL {
	/// <summary>
	/// Model Prediction Input Type
	/// </summary>
	public class hate_coremlInput : NSObject, IMLFeatureProvider
	{
		static readonly NSSet<NSString> featureNames = new NSSet<NSString> (
			new NSString ("input1")
		);

		MLMultiArray input1;

		/// <summary>
		///  as 1 1-dimensional array of doubles
		/// </summary>
		/// <value></value>
		public MLMultiArray Input1 {
			get { return input1; }
			set {
				if (value == null)
					throw new ArgumentNullException (nameof (value));

				input1 = value;
			}
		}

		public NSSet<NSString> FeatureNames {
			get { return featureNames; }
		}

		public MLFeatureValue GetFeatureValue (string featureName)
		{
			switch (featureName) {
			case "input1":
				return MLFeatureValue.Create (Input1);
			default:
				return null;
			}
		}

		public hate_coremlInput (MLMultiArray input1)
		{
			if (input1 == null)
				throw new ArgumentNullException (nameof (input1));

			Input1 = input1;
		}
	}

	/// <summary>
	/// Model Prediction Output Type
	/// </summary>
	public class hate_coremlOutput : NSObject, IMLFeatureProvider
	{
		static readonly NSSet<NSString> featureNames = new NSSet<NSString> (
			new NSString ("output1")
		);

		MLMultiArray output1;

		/// <summary>
		///  as 2 1-dimensional array of doubles
		/// </summary>
		/// <value></value>
		public MLMultiArray Output1 {
			get { return output1; }
			set {
				if (value == null)
					throw new ArgumentNullException (nameof (value));

				output1 = value;
			}
		}

		public NSSet<NSString> FeatureNames {
			get { return featureNames; }
		}

		public MLFeatureValue GetFeatureValue (string featureName)
		{
			switch (featureName) {
			case "output1":
				return MLFeatureValue.Create (Output1);
			default:
				return null;
			}
		}

		public hate_coremlOutput (MLMultiArray output1)
		{
			if (output1 == null)
				throw new ArgumentNullException (nameof (output1));

			Output1 = output1;
		}
	}

	/// <summary>
	/// Class for model loading and prediction
	/// </summary>
	public class hate_coreml : NSObject
	{
		readonly MLModel model;

		static NSUrl GetModelUrl ()
		{
			return NSBundle.MainBundle.GetUrlForResource ("hate_coreml", "mlmodelc");
		}

		public hate_coreml ()
		{
			NSError err;

			model = MLModel.Create (GetModelUrl (), out err);
		}

		hate_coreml (MLModel model)
		{
			this.model = model;
		}

		public static hate_coreml Create (NSUrl url, out NSError error)
		{
			if (url == null)
				throw new ArgumentNullException (nameof (url));

			var model = MLModel.Create (url, out error);

			if (model == null)
				return null;

			return new hate_coreml (model);
		}

		public static hate_coreml Create (MLModelConfiguration configuration, out NSError error)
		{
			if (configuration == null)
				throw new ArgumentNullException (nameof (configuration));

			var model = MLModel.Create (GetModelUrl (), configuration, out error);

			if (model == null)
				return null;

			return new hate_coreml (model);
		}

		public static hate_coreml Create (NSUrl url, MLModelConfiguration configuration, out NSError error)
		{
			if (url == null)
				throw new ArgumentNullException (nameof (url));

			if (configuration == null)
				throw new ArgumentNullException (nameof (configuration));

			var model = MLModel.Create (url, configuration, out error);

			if (model == null)
				return null;

			return new hate_coreml (model);
		}

		/// <summary>
		/// Make a prediction using the standard interface
		/// </summary>
		/// <param name="input">an instance of hate_coremlInput to predict from</param>
		/// <param name="error">If an error occurs, upon return contains an NSError object that describes the problem.</param>
		public hate_coremlOutput GetPrediction (hate_coremlInput input, out NSError error)
		{
			if (input == null)
				throw new ArgumentNullException (nameof (input));

			var prediction = model.GetPrediction (input, out error);

			if (prediction == null)
				return null;

			var output1Value = prediction.GetFeatureValue ("output1").MultiArrayValue;

			return new hate_coremlOutput (output1Value);
		}

		/// <summary>
		/// Make a prediction using the standard interface
		/// </summary>
		/// <param name="input">an instance of hate_coremlInput to predict from</param>
		/// <param name="options">prediction options</param>
		/// <param name="error">If an error occurs, upon return contains an NSError object that describes the problem.</param>
		public hate_coremlOutput GetPrediction (hate_coremlInput input, MLPredictionOptions options, out NSError error)
		{
			if (input == null)
				throw new ArgumentNullException (nameof (input));

			if (options == null)
				throw new ArgumentNullException (nameof (options));

			var prediction = model.GetPrediction (input, options, out error);

			if (prediction == null)
				return null;

			var output1Value = prediction.GetFeatureValue ("output1").MultiArrayValue;

			return new hate_coremlOutput (output1Value);
		}

		/// <summary>
		/// Make a prediction using the convenience interface
		/// </summary>
		/// <param name="input1"> as 1 1-dimensional array of doubles</param>
		/// <param name="error">If an error occurs, upon return contains an NSError object that describes the problem.</param>
		public hate_coremlOutput GetPrediction (MLMultiArray input1, out NSError error)
		{
			var input = new hate_coremlInput (input1);

			return GetPrediction (input, out error);
		}

		/// <summary>
		/// Make a prediction using the convenience interface
		/// </summary>
		/// <param name="input1"> as 1 1-dimensional array of doubles</param>
		/// <param name="options">prediction options</param>
		/// <param name="error">If an error occurs, upon return contains an NSError object that describes the problem.</param>
		public hate_coremlOutput GetPrediction (MLMultiArray input1, MLPredictionOptions options, out NSError error)
		{
			var input = new hate_coremlInput (input1);

			return GetPrediction (input, options, out error);
		}
	}
}
