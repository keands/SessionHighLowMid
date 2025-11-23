# SessionHighLowMid

A Quantower custom indicator that visualizes **High**, **Low**, and **Mid (average)** levels for the three main trading sessions: Asian, London, and US.

## Features

- Plots the High, Low, and Mid lines for each session directly on your chart
- **Supported sessions:**  
  - Asian: 23:00–07:00 UTC  
  - London: 07:00–16:00 UTC  
  - US PM: 12:00–21:00 UTC
- Easily toggle which sessions to display
- Configurable colors and line styles for each session and level
- Real-time calculation of session levels, automatically resets at each new session
- The Mid line is dynamically calculated as the average of the High and Low values per session

## Installation

1. Download the `SessionHighLowMid.zip` file.
2. Unzip to {QUANTOWER_INSTALLATION_PATH]\Settings\Scripts\Indicators\
3. Select `SessionHighLowMid` into your custom indicators in Quantower

## Usage

- Configure which sessions to show (Asian, London, US).
- Set custom colors and line widths for High, Low, and Mid levels in each session.
- The indicator will update session boundaries automatically as market time progresses.

## Parameters

- `ShowAsianSession` (bool): Show/hide Asian session (default: true)
- `ShowLondonSession` (bool): Show/hide London session (default: true)
- `ShowUSSession` (bool): Show/hide US session (default: true)
- `AsianHighColor`, `AsianLowColor`, `AsianMiddleColor`
- `LondonHighColor`, `LondonLowColor`, `LondonMiddleColor`
- `USHighColor`, `USLowColor`, `USMiddleColor`
- `LineWidth` (int): Thickness of the plotted lines

## Example

Once added to your chart, the indicator displays the High, Low, and Mid levels for the selected sessions as colored lines, updating automatically each day/UTC session.

---

**Ideal for intraday traders, scalpers, and anyone needing a clear visualization of session price boundaries.**

---

