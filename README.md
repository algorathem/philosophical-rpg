# Zylar's World - WebGL Build

Welcome to **Zylar's World**! This README provides instructions on how you can play the game locally.

## Requirements:
- **Python** (version 3.x or above) installed on your computer.
- A **local development environment** (such as Python's HTTP server) to serve the game files.
- **Git LFS** installed (required for downloading large asset files properly).

## How to Play Locally:

To run the game locally on your computer, you need to use a local HTTP server. This guide explains how to do this using Python.

### 1. **Clone the Repository with Git LFS:**

First, ensure [Git LFS](https://git-lfs.github.com/) is installed on your system.

Then run the following commands:

```bash
git lfs install
git clone https://github.com/algorathem/philosophical-rpg.git
```

### 2. **Navigate to the Game Directory:**
   - Open the **command prompt** (Windows) or **terminal** (Mac/Linux).
   - Use the `cd` (change directory) command to go to the folder where the WebGL build files are located (where `index.html` is).
   
   Example:
   ```bash
   cd path/to/your/webgl/build/folder
   ```
### 3. **Start the HTTP Server:**

Once you're in the correct directory, run the following command:

```bash
python -m http.server 8000
```
This will start a local server at `http://localhost:8000`.
### 4. **Open the Game in Your Browser:**
Open your preferred browser (Chrome, Firefox, etc.).

Go to `http://localhost:8000` in the address bar.

The game should load, and you can start playing!
## Troubleshooting:

- **CORS Issues**: If you face issues with assets not loading, ensure you're using the local server as described. WebGL requires HTTP or HTTPS to load assets properly.

- **Python Not Installed**: If Python is not installed on your system, [download Python here](https://www.python.org/downloads/).

- **Port Already in Use**: If port `8000` is occupied by another program, change the port number by running:

```bash
python -m http.server 8080
```
