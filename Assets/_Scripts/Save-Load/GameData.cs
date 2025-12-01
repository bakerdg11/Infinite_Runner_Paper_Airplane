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
        edrMaxLevel = 9;
        lcsMaxLevel = 10;

        pedLengthMaxLevel = 6;
        pedAmmoMaxLevel = 2;

        boostLengthMaxLevel = 6;
        boostAmmoMaxLevel = 3;

        invincibilityLengthMaxLevel = 6;
        invincibilityAmmoMaxLevel = 5;

        dashAmmoMaxLevel = 4;
        missileAmmoMaxLevel = 10;
    }
}