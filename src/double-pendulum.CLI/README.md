# src/double-pendulum.CLI

Console application for running the double pendulum simulation in a terminal. Outputs position data in real time and optionally exports to Json locally and/or uploads to **MongoDB Atlas** server. Also the project part used for the Docker image.

## Contents

- `ConsolePrompt.cs`: Yes/No user prompting loop
- `ExportDTO.cs`: Data transfer object for export
- `JsonExporter.cs`: Handles Json export to `output/`
- `MongoExporter.cs`: Handles export to **MongoDB Atlas**
- `ParameterInputHandler.cs`: Prompts the user to enter physical parameters (lengths, masses, initial angles, damping) with range validation
- `Program.cs`: Entry point
- `Simulation.cs`: Runs the simulation loop for a selected duration, prints `(t, X1, Y1, X2, Y2)` to the console at each step, returns DTO.
