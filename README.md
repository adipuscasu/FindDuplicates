# FindDuplicates

> Console application to identify and remove duplicate files from the subfolders of a specified folder.

**FindDuplicates** is a lightweight C# .NET console utility designed to help you locate, analyze, and optionally remove duplicate files within a directory tree. It simplifies duplicate file cleanup in large folders — especially useful for drives with many similar images, documents, or media files.

---

## 🚀 Features

- 🔍 **Recursive scanning** of all subfolders for duplicate files
- 📁 **File hashing comparison** to reliably detect duplicates
- 🗑️ **Optional delete mode** to remove duplicated content
- 📊 Detailed console output for matched results
- 💡 Designed as a simple utility — easy to customize and extend

---

## 🧠 How It Works

FindDuplicates reads all files inside a given directory and:

1. Computes a content hash for each file
2. Groups files that share identical hashes
3. Outputs duplicate groups to the console
4. Offers an optional cleanup (delete) mode

This approach avoids relying on file names or timestamps, using actual content comparison instead.

---

## 📥 Requirements

- **.NET 10.0 or later** (or appropriate .NET SDK matching project)
- Windows / macOS / Linux — any environment that supports .NET

---

## 🛠️ Usage

### 1. Build

Clone the repo and build:

```bash
git clone https://github.com/adipuscasu/FindDuplicates.git
cd FindDuplicates
dotnet build
```

**Note:** This project requires .NET 10.0 SDK or later. If you don't have it installed, download it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download).

### 2. Run

You can run the application in two ways:

#### Option A: Using `dotnet run` (Recommended for development)

```bash
cd FindDuplicates
dotnet run find "C:\Images"      # Scan for duplicates in C:\Images
dotnet run remove "C:\Images"    # Scan and remove duplicates in C:\Images
```

#### Option B: Using the built executable

```bash
cd .\src\FindDuplicates\bin\Debug\net10.0
.\FindDuplicates.exe find "C:\Images"      # Scan for duplicates in C:\Images
.\FindDuplicates.exe remove "C:\Images"    # Scan and remove duplicates in C:\Images
```

### 3. Command Options

#### `find` - Scan for duplicates
```bash
dotnet run find "C:\Images"
```

#### `remove` - Scan and remove duplicates
```bash
dotnet run remove "C:\Images"
```

#### `--extensions` / `-e` - Filter by file extension
Filter files by extension when finding or removing duplicates:

```bash
# Find duplicate JPG and PNG images only
dotnet run find "C:\Images" --extensions jpg,png

# Remove duplicate MP3 files only
dotnet run remove "C:\Music" --extensions mp3

# Using short form
dotnet run find "C:\Documents" -e pdf,docx
```

### 4. Getting Help

Display help information:

```bash
dotnet run -- --help
```

This shows all available commands and options.

---

## 📚 Examples

### Basic Duplicate Detection
```bash
# Find duplicates in a folder
dotnet run find "C:\Photos"

# Find duplicates in current directory
dotnet run find .
```

### Remove Duplicates
```bash
# Remove duplicates (files will be deleted!)
dotnet run remove "C:\Documents"
```

### Filter by File Extension
```bash
# Find duplicate images only
dotnet run find "C:\Media" --extensions jpg,png,gif

# Find duplicate documents only
dotnet run find "C:\Documents" -e pdf,docx,txt

# Remove duplicate audio files
dotnet run remove "C:\Music" --extensions mp3,wav,flac
```

### Real-World Scenarios
```bash
# Clean up duplicate screenshots
dotnet run remove "C:\Users\YourName\Pictures\Screenshots" -e png,jpg

# Find duplicate code files for review
dotnet run find "C:\Projects" --extensions cs,js,py,java

# Remove duplicate video files
dotnet run remove "D:\Videos" --extensions mp4,mkv,avi
```

---

## 🧪 Testing

This project includes a comprehensive test suite in the `FindDuplicatesTests` project:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## 🔧 Troubleshooting

### Common Issues

**"The specified path does not exist"**
- Ensure the directory path is correct and accessible
- Use absolute paths (e.g., `C:\Images` instead of `Images`)

**"Access denied" when removing files**
- Run the terminal as Administrator
- Check file permissions and ensure files aren't in use

**No duplicates found despite having similar files**
- Check file extensions with `--extensions` if filtering
- Verify files have different content (not just different names)

**Build fails with "SDK not found"**
- Install .NET 10.0 SDK from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
- Verify installation: `dotnet --version`

---

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.