# NeuroAvatar: Real-time BCI Control System (Phase 2)

This project represents **Phase 2** of the EEG Decoding initiative, transitioning from static binary classification to a **Real-time 4-Class Control System** using Deep Learning and Unity 3D.

## Project Overview
While Phase 1 focused on offline accuracy using CSP+LDA, Phase 2 implements an end-to-end real-time loop. The system uses **EEGNet (Deep Learning)** to decode 4 distinct Motor Imagery tasks and translates them into immediate avatar movements.

### Key Features
* **Multi-Class Decoding:** Classifies 4 actions (Left Hand, Right Hand, Feet, Tongue).
* **Deep Learning Core:** Utilizes **EEGNet** (CNN) for robust feature extraction from raw EEG.
* **Real-time Bridge:** Low-latency UDP communication between Python (Backend) and Unity (Frontend).
* **State Machine Logic:** Implements "Latching Control" in Unity for complex, multi-limb avatar actuation.

## Technical Stack
* **AI Backend:** Python 3.x, TensorFlow/Keras, NumPy.
* **Simulation:** Unity 3D, C# (UDP Socket & Animation Rigging).
* **Dataset:** BCI Competition IV 2a.

## System Architecture
1. **Input:** Raw EEG signals (22 Channels).
2. **Processing:** EEGNet Model predicts user intent (Python).
3. **Transmission:** Commands sent via UDP Socket (Localhost).
4. **Execution:** Unity receives signals and drives the 3D Avatar's skeleton.

## Project Roadmap
[x] **Phase 1: Foundation & Baseline Decoding** (CSP + LDA)<br>
*[x] **Phase 2: Deep Learning & Real-time 3D Integration** (EEGNet, Unity, 4-Class)*<br>
[ ] **Phase 3: Multi-Label Expansion** (Simultaneous independent limb control via High-Density EEG)
