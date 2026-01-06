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

- **.NET 6.0 or later** (or appropriate .NET SDK matching project)  
- Windows / macOS / Linux — any environment that supports .NET

---

## 🛠️ Usage

### 1. Build

Clone the repo and build:

```bash
git clone https://github.com/adipuscasu/FindDuplicates.git
cd FindDuplicates
dotnet build
