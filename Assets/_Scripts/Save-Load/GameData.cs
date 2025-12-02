using UnityEngine;
using System;

[Serializable]
public class GameData
{
    // Currency
    public int totalCredits;
    public int totalAbilityPoints;

    // Stats
    public int edrCurrentLevel;
    public int edrMaxLevel;
    public int lcsCurrentLevel;
    public int lcsMaxLevel;

    // Pause Energy Depletion
    public int pedLengthCurrentLevel;
    public int pedLengthMaxLevel;
    public int pedAmmoCurrentLevel;
    public int pedAmmoMaxLevel;
    // Boost
    public int boostLengthCurrentLevel;
    public int boostLengthMaxLevel;
    public int boostAmmoCurrentLevel;
    public int boostAmmoMaxLevel;
    // Invincibility
    public int invincibilityLengthCurrentLevel;
    public int invincibilityLengthMaxLevel;
    public int invincibilityAmmoCurrentLevel;
    public int invincibilityAmmoMaxLevel;
    // Dash
    public int dashAmmoCurrentLevel;
    public int dashAmmoMaxLevel;
    // Missile
    public int missileAmmoCurrentLevel;
    public int missileAmmoMaxLevel;

    // Default constructor for a fresh save
    public GameData()
    {
        totalCredits = 0;
        totalAbilityPoints = 0;

        edrCurrentLevel = 0;
        edrMaxLevel = 9;
        lcsCurrentLevel = 0;
        lcsMaxLevel = 10;

        pedLengthCurrentLevel = 0;
        pedLengthMaxLevel = 6;
        pedAmmoCurrentLevel = 0;
        pedAmmoMaxLevel = 2;

        boostLengthCurrentLevel = 0;
        boostLengthMaxLevel = 6;
        boostAmmoCurrentLevel = 0;
        boostAmmoMaxLevel = 3;

        invincibilityLengthCurrentLevel = 0;
        invincibilityLengthMaxLevel = 6;
        invincibilityAmmoCurrentLevel = 0;
        invincibilityAmmoMaxLevel = 5;

        dashAmmoCurrentLevel = 0;
        dashAmmoMaxLevel = 4;
        missileAmmoCurrentLevel = 0;
        missileAmmoMaxLevel = 10;
    }
}