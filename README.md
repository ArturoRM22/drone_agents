# TC2008B: Multi-Agent System Modeling With Computational Graphics

## Overview

This repository contains the code and documentation for the integrative activity which consists of an Intruder Detection Simulation. The project is divided into three main components:

### 1. Multi-Agent Systems

In this scenario, you manage a warehouse that is monitored by a drone equipped with advanced cameras and sensors. The drone is tasked with patrolling a routine path throughout the warehouse, scanning for any signs of intrusion. The focus is on simulating the drone's behavior as it follows its path, while a police officer monitors the warehouse's camera feeds. Upon detecting an intruder via the cameras, the officer quickly deploys the drone to the exact location to confirm the presence of the intruder. The emphasis is on optimizing the drone's response time, improving its surveillance capabilities, and proposing strategies to enhance the overall security of the warehouse.

### 2. Computational Graphics

This section involves creating a 3D model of the warehouse, the drone, the intruder and the officer. The project covers texture mapping, lighting, and basic collision detection. The goal is to visually represent the Agents as they perform their tasks in the simulated environment, adding a layer of realism to the simulation.

### 3. Computer Vision
	
The drone uses YOLO to monitor the warehouse, identifying objects and intruders in real time. When the officer spots an anomaly on camera, the drone quickly confirms the intruder's presence for a swift response.

## Project Structure

- *Part 1: Multi-Agent Systems*
  - Documentation: PDF with agent and environment properties, success metrics, class diagrams, and ontology diagrams.
  - Code: Implemented simulation code for the multi-agent challenge.
  
- *Part 2: Computational Graphics*
  - File: .unitypackage containing all necessary assets for 3D rendering and simulation.

- *Part 3: Computer Vision*
  - Vision models and integration code.

## How to Run

1. Clone the repository.
2. Follow the instructions in the Part 1 and Part 2 directories to set up and run the simulations.
3. Use Unity to open the .unitypackage file for the 3D simulation.

## Notes

- The project simulates a warehouse with MxN grid spaces, a dron, cameras, an intruder and a guard.
- Maximum execution time is limited by either steps or seconds.

## Colaboradores
- Arturo Ramos Martínez A01643269
- Adolfo Hernández Signoret A01637184
- Bryan Ithan Landín Lara A01636271
- Diego Enrique Vargas Ramírez A01635782
- Luis Fernando Cuevas Arroyo A01637254

## Muestras Youtube de simulacion
- Vision Yolo: https://youtu.be/VhlHuHSQZBI 
- Simulacion agente: https://youtu.be/FG7tl6h5HHA

## Proceso para clonar
- git clone https://github.com/ArturoRM22/drone_agents.git

## Proceso para correr servidor fastapi con el modelo de AgentPy
- cd drone_agents/droneAPI {venv recomendado}
- pip install -r requirements.txt
- python app.py
- Correr unity

## Proceso para correr el servidor de la visión computacional (Yolo)
- git clone https://github.com/Nuclea-Solutions/tec-2024B.git
- cd tec-2024B\examples\unity-server
- python server.py
- Correr unity

## Usar unity pack
- Creas proyecto en unity
- En la pestaña de assets importas el pack Simulation.unitypackage
- Corres

## Jupyter notebook version (gráficas (parametros hardcodeados))
- Asegurarse de tener instalado anaconda o algun otro interprete de Jupyter notebooks  (O google colab)
- Abrir el archivo Current_CamerasModel.ipynb y corres
- Verificar las instalaciones (el primer chunk)

