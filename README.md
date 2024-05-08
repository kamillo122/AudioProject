# C# WPF Player Application Documentation

## Introduction
This documentation provides an overview of the C# WPF Player Application, detailing its features, classes, and usage.

## Features
- Audio file playback
- Waveform visualization
- Equalizer
- Audio queue management

## Classes

### 1. `MainWindow.xaml.cs`
This class represents the main window of the application.

#### Methods
- `MainWindow()`: Constructor method.
- `LoadAudioFile(string filePath)`: Loads an audio file for playback.
- `Play()`: Begins playback of the loaded audio file.
- `Pause()`: Pauses playback of the audio file.
- `Stop()`: Stops playback of the audio file.
- `UpdateVisualization()`: Updates the waveform visualization during playback.

### 2. `SettingsWindow.xaml.cs`
This class represents the settings window of the application.

#### Methods
- `SettingsWindow()`: Constructor method.
- `ApplySettings()`: Applies the settings configured by the user.

### 3. `Visualization.cs`
This class is responsible for generating waveform visualizations of audio files.

### 4. `Equalizer.cs`
This class implements the equalizer functionality.

### 5. `AudioQueue.cs`
This class manages the audio queue for playback.

### 6. `AudioPlayer.cs`
This class handles audio file playback.

### 7. `IWaveFormRenderer.cs`
This interface defines methods for rendering waveform visualizations.

### 8. `EqualizerBand.cs`
This class represents an equalizer band.

## Usage
1. **Loading an audio file**
   ```csharp
   mainWindow.LoadAudioFile(filePath);
   ```
![audioproject](https://github.com/kamillo122/AudioProject/assets/67054069/8d5733d8-8565-4572-a483-db53d3d94fa6)
