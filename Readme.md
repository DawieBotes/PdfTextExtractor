# Pdf Text Extractor

Released EXE in Release folder.

To run or build code:
```
- dotnet run c:\Temp\1.pdf
- dotnet publish -c Release -r win-x64 --self-contained false
```

## Description
Pdf Text Extractor is a command-line tool for extracting text from PDFs while preserving line structures and distinguishing columns. It replaces spaces between columns with a customizable separator and provides additional debugging options for analyzing word positions.

## Features
- Extracts text from a PDF while maintaining line breaks.
- Identifies columns and replaces large spaces with a user-defined separator (default: `###`).
- Provides debugging options to display word coordinates.
- Allows customization of column and row detection thresholds.
- Supports an option to suppress comments such as "Processing PDF" and "End of page."

## Usage

### Basic Command
```
PdfTextExtractor <PDF file path>
```

### Available Parameters
- `--separator <string>`: Defines the separator for column breaks (default: `###`).
- `--yTolerance <double>`: Sets the vertical tolerance for detecting lines (default: `3.0`).
- `--columnThreshold <double>`: Sets the horizontal gap threshold for detecting columns (default: `15.0`).
- `--show-coordinates`: Displays word coordinates next to extracted text for debugging.
- `--no-comments`: Removes extra comments such as "Processing PDF" and "End of page."

### Examples
#### Extract text using default settings:
```
PdfTextExtractor document.pdf
```

#### Extract text with a custom column separator:
```
PdfTextExtractor document.pdf --separator "|"
```

#### Extract text while showing word coordinates:
```
PdfTextExtractor document.pdf --show-coordinates
```

#### Extract text with adjusted column and row detection settings:
```
PdfTextExtractor document.pdf --yTolerance 2.5 --columnThreshold 10.0
```

#### Extract text without additional comments:
```
PdfTextExtractor document.pdf --no-comments
```

## Requirements
- .NET Runtime (if not running as a self-contained executable)
- A valid PDF file as input

## License
This tool is provided as-is. Modify and distribute as needed.

## Author
Developed by [Your Name or Organization].

