# WarCardGame-Unity-Evaluation
Unity-Evaluation for Tamatem Company

# Unity Card Game Project

## Introduction
This is a Unity-based card game developed as part of a technical assessment. The game integrates with a REST API to fetch card data and features dependency injection, asynchronous operations, animations, and sound effects. The project showcases clean, maintainable code and a well-structured design.

## Code Structure and Logic
- Architecture:
  - The project uses dependency injection to manage components like the game manager and services, ensuring loose coupling and better organization. Dependencies are injected via a `Bootstrap` script in the `MainScene`.
  - UniTask is used for asynchronous operations, such as fetching data from the REST API and controlling animations, improving performance and readability.
  - Newtonsoft.Json handles serialization and deserialization of API responses for card data.
- Game Logic:
  - The core logic is managed by a `GameManager` class, which handles card drawing, comparison, and game outcomes. The `DeckApiService` class integrates with the REST API using async methods.
  - One scene (`MainScene`) contains a manager and a bootstrapper to initialize dependencies and maintain order, avoiding null references in the Inspector.

## Optional or Extra Features
- UniTask: Enhances asynchronous handling for API calls and animations.
- Animations with DOTween: Smooth animations for card movements and reveals are implemented using DOTween and Unity’s Timeline.
- Sound Effects: Includes sounds for card flips and game events, created with Unity’s built-in audio tools.
- Dependency Injection: A lightweight custom solution ensures modularity and prevents null references by assigning dependencies in the Inspector.
- Design: Uses primitive Unity sprites for cards and UI, animated with DOTween.

## Instructions for Running and Testing
- Unity Version: 6000.0.48f1
- Dependencies:
  - UniTask (via Package Manager)
  - DOTween (via Package Manager)
  - Newtonsoft.Json (via Package Manager)
- Setup Instructions:
  1. Clone the repository from GitHub:
     ```bash
     git clone <your-github-repo-url>
