// Copyright QUANTOWER LLC. © 2017-2023. All rights reserved.

using System;
using System.Drawing;
using TradingPlatform.BusinessLayer;

namespace SessionHighLowMid
{
    /// <summary>
    /// An example of blank indicator. Add your code, compile it and use on the charts in the assigned trading terminal.
    /// Information about API you can find here: http://api.quantower.com
    /// Code samples: https://github.com/Quantower/Examples
    /// </summary>
	public class SessionHighLowMid : Indicator
    {
        #region Parameters
        [InputParameter("Show Asian Session")]
        public bool ShowAsianSession = true;

        [InputParameter("Show London Session")]
        public bool ShowLondonSession = true;

        [InputParameter("Show US Session")]
        public bool ShowUSSession = true;

        [InputParameter("Asian High Color")]
        public Color AsianHighColor = Color.Purple;

        [InputParameter("Asian Low Color")]
        public Color AsianLowColor = Color.Purple;

        [InputParameter("Asian Middle Color")]
        public Color AsianMiddleColor = Color.MediumPurple;

        [InputParameter("London High Color")]
        public Color LondonHighColor = Color.White;

        [InputParameter("London Low Color")]
        public Color LondonLowColor = Color.White;

        [InputParameter("London Middle Color")]
        public Color LondonMiddleColor = Color.WhiteSmoke;

        [InputParameter("US High Color")]
        public Color USHighColor = Color.Blue;

        [InputParameter("US Low Color")]
        public Color USLowColor = Color.Blue;

        [InputParameter("US Middle Color")]
        public Color USMiddleColor = Color.CornflowerBlue;

        [InputParameter("Line Width")]
        public int LineWidth = 2;
        #endregion

        #region Private Fields
        private double asianSessionHigh;
        private double asianSessionLow;

        private double londonSessionHigh = double.MinValue;
        private double londonSessionLow = double.MaxValue;

        private double usSessionHigh = double.MinValue;
        private double usSessionLow = double.MaxValue;

        private DateTime currentAsianSessionStart = DateTime.MinValue;
        private DateTime currentLondonSessionStart = DateTime.MinValue;
        private DateTime currentUSSessionStart = DateTime.MinValue;

        private bool isNewAsianSession = false;
        private bool isNewLondonSession = false;
        private bool isNewUSSession = false;
        #endregion

        #region Indicator Lines
        private LineSeries asianHighLine;
        private LineSeries asianLowLine;
        private LineSeries asianMiddleLine;
        private LineSeries londonHighLine;
        private LineSeries londonLowLine;
        private LineSeries londonMiddleLine;
        private LineSeries usHighLine;
        private LineSeries usLowLine;
        private LineSeries usMiddleLine;
        #endregion

        public SessionHighLowMid()
            : base()
        {
            // Defines indicator's name and description.
            Name = "Session High Low Middle";
            Description = "Displays high, middle, and low levels for Asian, London, and US PM trading sessions";
            SeparateWindow = false;

        }

      
        protected override void OnInit()
        {
            // Initialize indicator lines
            if (ShowAsianSession)
            {
                asianHighLine = AddLineSeries("Asian High", Color.Transparent, LineWidth, LineStyle.Solid);
                asianHighLine.Color = AsianHighColor;

                asianLowLine = AddLineSeries("Asian Low", Color.Transparent, LineWidth, LineStyle.Solid);
                asianLowLine.Color = AsianLowColor;

                asianMiddleLine = AddLineSeries("Asian Middle", Color.Transparent, LineWidth, LineStyle.Dash);
                asianMiddleLine.Color = AsianMiddleColor;
            }

            if (ShowLondonSession)
            {
                londonHighLine = AddLineSeries("London High", Color.Transparent, LineWidth, LineStyle.Solid);
                londonHighLine.Color = LondonHighColor;

                londonLowLine = AddLineSeries("London Low", Color.Transparent, LineWidth, LineStyle.Solid);
                londonLowLine.Color = LondonLowColor;

                londonMiddleLine = AddLineSeries("London Middle", Color.Transparent, LineWidth, LineStyle.Dash);
                londonMiddleLine.Color = LondonMiddleColor;
            }

            if (ShowUSSession)
            {
                usHighLine = AddLineSeries("US High", Color.Transparent, LineWidth, LineStyle.Solid);
                usHighLine.Color = USHighColor;

                usLowLine = AddLineSeries("US Low", Color.Transparent, LineWidth, LineStyle.Solid);
                usLowLine.Color = USLowColor;

                usMiddleLine = AddLineSeries("US Middle", Color.Transparent, LineWidth, LineStyle.Dash);
                usMiddleLine.Color = USMiddleColor;
            }
        }

        protected override void OnUpdate(UpdateArgs args)
        {
            if (Count < 1)
                return;

            DateTime currentTime = Time(0);
            double currentHigh = High(0);
            double currentLow = Low(0);

            // Check for new sessions and update high/low levels
            CheckAndUpdateSessions(currentTime, currentHigh, currentLow);

            // Update indicator line values
            UpdateIndicatorLines();
        }

        private void CheckAndUpdateSessions(DateTime currentTime, double currentHigh, double currentLow)
        {
            // Convert to UTC for consistent session detection
            DateTime utcTime = currentTime.ToUniversalTime();
            TimeSpan timeOfDay = utcTime.TimeOfDay;

            // Asian Session: 23:00 UTC (previous day) - 07:00 UTC
            bool isAsianSession = (timeOfDay >= new TimeSpan(23, 0, 0)) || (timeOfDay <= new TimeSpan(6, 0, 0));

            // London Session: 07:00 UTC - 16:00 UTC
            bool isLondonSession = (timeOfDay >= new TimeSpan(7, 0, 0)) && (timeOfDay <= new TimeSpan(10, 0, 0));

            // US PM Session: 12:00 UTC - 21:00 UTC
            bool isUSSession = (timeOfDay >= new TimeSpan(18, 0, 0)) && (timeOfDay <= new TimeSpan(21, 0, 0));

            // Asian Session Logic
            if (ShowAsianSession && isAsianSession)
            {
                DateTime sessionStart = GetAsianSessionStart(utcTime);
                if (currentAsianSessionStart != sessionStart)
                {
                    currentAsianSessionStart = sessionStart;
                    asianSessionHigh = currentHigh;
                    asianSessionLow = currentLow;
                    isNewAsianSession = true;
                }
                else
                {
                    asianSessionHigh = Math.Max(asianSessionHigh, currentHigh);
                    asianSessionLow = Math.Min(asianSessionLow, currentLow);
                }
            }

            // London Session Logic
            if (ShowLondonSession && isLondonSession)
            {
                DateTime sessionStart = GetLondonSessionStart(utcTime);
                if (currentLondonSessionStart != sessionStart)
                {
                    currentLondonSessionStart = sessionStart;
                    londonSessionHigh = currentHigh;
                    londonSessionLow = currentLow;
                    isNewLondonSession = true;
                }
                else
                {
                    londonSessionHigh = Math.Max(londonSessionHigh, currentHigh);
                    londonSessionLow = Math.Min(londonSessionLow, currentLow);
                }
            }

            // US Session Logic
            if (ShowUSSession && isUSSession)
            {
                DateTime sessionStart = GetUSSessionStart(utcTime);
                if (currentUSSessionStart != sessionStart)
                {
                    currentUSSessionStart = sessionStart;
                    usSessionHigh = currentHigh;
                    usSessionLow = currentLow;
                    isNewUSSession = true;
                }
                else
                {
                    usSessionHigh = Math.Max(usSessionHigh, currentHigh);
                    usSessionLow = Math.Min(usSessionLow, currentLow);
                }
            }
        }

        private void UpdateIndicatorLines()
        {
            // Update Asian session lines
            if (ShowAsianSession && asianHighLine != null && asianLowLine != null && asianMiddleLine != null)
            {
                if (asianSessionHigh != double.MinValue)
                    asianHighLine[0] = asianSessionHigh;

                if (asianSessionLow != double.MaxValue)
                    asianLowLine[0] = asianSessionLow;

                if (asianSessionHigh != double.MinValue && asianSessionLow != double.MaxValue)
                    asianMiddleLine[0] = (asianSessionHigh + asianSessionLow) / 2.0;
            }

            // Update London session lines
            if (ShowLondonSession && londonHighLine != null && londonLowLine != null && londonMiddleLine != null)
            {
                if (londonSessionHigh != double.MinValue)
                    londonHighLine[0] = londonSessionHigh;

                if (londonSessionLow != double.MaxValue)
                    londonLowLine[0] = londonSessionLow;

                if (londonSessionHigh != double.MinValue && londonSessionLow != double.MaxValue)
                    londonMiddleLine[0] = (londonSessionHigh + londonSessionLow) / 2.0;
            }

            // Update US session lines
            if (ShowUSSession && usHighLine != null && usLowLine != null && usMiddleLine != null)
            {
                if (usSessionHigh != double.MinValue)
                    usHighLine[0] = usSessionHigh;

                if (usSessionLow != double.MaxValue)
                    usLowLine[0] = usSessionLow;

                if (usSessionHigh != double.MinValue && usSessionLow != double.MaxValue)
                    usMiddleLine[0] = (usSessionHigh + usSessionLow) / 2.0;
            }
        }

        private DateTime GetAsianSessionStart(DateTime utcTime)
        {
            // Asian session starts at 23:00 UTC
            if (utcTime.TimeOfDay >= new TimeSpan(23, 0, 0))
                return new DateTime(utcTime.Year, utcTime.Month, utcTime.Day, 23, 0, 0, DateTimeKind.Utc);
            else
                return new DateTime(utcTime.Year, utcTime.Month, utcTime.Day, 23, 0, 0, DateTimeKind.Utc).AddDays(-1);
        }

        private DateTime GetLondonSessionStart(DateTime utcTime)
        {
            // London session starts at 07:00 UTC
            return new DateTime(utcTime.Year, utcTime.Month, utcTime.Day, 7, 0, 0, DateTimeKind.Utc);
        }

        private DateTime GetUSSessionStart(DateTime utcTime)
        {
            // US session starts at 12:00 UTC
            return new DateTime(utcTime.Year, utcTime.Month, utcTime.Day, 12, 0, 0, DateTimeKind.Utc);
        }

    }
}
