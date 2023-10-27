using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEnemyController : MonoBehaviour
{
    // Define states for the rotation direction
    private enum RotationState
    {
        RotatingClockwise,
        RotatingCounterClockwise
    }

    // Current rotation state of the enemy
    private RotationState currentState;

    // Initial rotation when the game starts
    private Quaternion initialRotation;
    // Target rotation when rotating clockwise
    private Quaternion clockwiseRotation;
    // Target rotation when rotating counterclockwise (back to initial rotation)
    private Quaternion counterClockwiseRotation;
    
    // Speed of the rotation (can be adjusted in Inspector)
    public float rotationSpeed = 0.05f; 
    // Angle to rotate (can be adjusted in Inspector)
    public float rotationAngle = 180f;

    // Progress of the current rotation (ranges from 0 to 1)
    private float rotationProgress = 0;

    private void Start()
    {
        // Capture the initial rotation when the game starts
        initialRotation = transform.rotation;
        // Calculate the target rotation based on the desired angle
        clockwiseRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, rotationAngle, 0));
        // Set counterclockwise rotation to the initial rotation
        counterClockwiseRotation = initialRotation;
        
        // Start rotating clockwise by default
        currentState = RotationState.RotatingClockwise;
    }

    private void Update()
    {
        // Continuously rotate the enemy based on the state
        RotateEnemy();
    }

    private void RotateEnemy()
    {
        switch (currentState)
        {
            case RotationState.RotatingClockwise:
                // Increase the rotation progress based on speed and time
                rotationProgress += rotationSpeed * Time.deltaTime;
                // Interpolate between the initial and target rotations
                transform.rotation = Quaternion.Lerp(initialRotation, clockwiseRotation, rotationProgress);

                // Check if the rotation is complete
                if (rotationProgress >= 1)
                {
                    // Reset progress and switch to counter-clockwise rotation
                    rotationProgress = 0;
                    currentState = RotationState.RotatingCounterClockwise;
                }
                break;
            case RotationState.RotatingCounterClockwise:
                // Similar logic as above but for counter-clockwise rotation
                rotationProgress += rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(clockwiseRotation, counterClockwiseRotation, rotationProgress);

                if (rotationProgress >= 1)
                {
                    rotationProgress = 0;
                    currentState = RotationState.RotatingClockwise;
                }
                break;
        }
    }
}


