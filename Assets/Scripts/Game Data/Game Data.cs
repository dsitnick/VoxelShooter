using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData {

    public struct Game {
        public List<Character> characters;
        public List<Weapon> weapons;
    }

    public struct Character {
        private const int WEAPON_COUNT = 4;

        public float healthMax, shieldMax, shieldRegen, shieldDelay;
        public float speed, jumpHeight;

        public int headModel, bodyModel, handModel, footModel;
        public int[] weapons;

        public Character (float healthMax, float shieldMax, float shieldRegen, float shieldDelay, float speed, float jumpHeight,
            int headModel, int bodyModel, int handModel, int footModel, int[] weapons) {
            this.healthMax = healthMax;
            this.shieldMax = shieldMax;
            this.shieldRegen = shieldRegen;
            this.shieldDelay = shieldDelay;
            this.speed = speed;
            this.jumpHeight = jumpHeight;
            this.headModel = headModel;
            this.bodyModel = bodyModel;
            this.handModel = handModel;
            this.footModel = footModel;
            this.weapons = new int[WEAPON_COUNT];
            for (int i = 0; i < WEAPON_COUNT; i++) { this.weapons[i] = weapons[i]; }
        }

        public static Character Default = new Character (100, 100, 20, 1.5f, 50, 2, -1, -1, -1, -1, new int[] { -1, -1, -1, -1 });

        /*public Character (Character other) {
            healthMax = other.healthMax;
            shieldMax = other.shieldMax;
            shieldRegen = other.shieldRegen;
            shieldDelay = other.shieldDelay;
            speed = other.speed;
            jumpHeight = other.jumpHeight;
            headModel = other.headModel;
            bodyModel = other.bodyModel;
            handModel = other.handModel;
            footModel = other.footModel;
            weapons = new int[WEAPON_COUNT];
            for (int i = 0; i < WEAPON_COUNT; i++) { weapons[i] = other.weapons[i]; }
        }*/
    }

    public struct Weapon {
        public enum Type { Auto, Sniper, Scatter, Projectile, Explosive, Spray, Beam, Melee }
        public Damage damage;
        public int clipSize;
        public float reloadTime, fireRate;
        public int model;
        public Type type;

        public Weapon (Damage damage, int clipSize, float reloadTime, float fireRate, int model, Type type) {
            this.damage = damage;
            this.clipSize = clipSize;
            this.reloadTime = reloadTime;
            this.fireRate = fireRate;
            this.model = model;
            this.type = type;
        }
    }

    public struct AutoWeapon {
        public float falloffRange, falloffAmount, headshotScale, spread;
        public Weapon weapon;

        public AutoWeapon(Damage damage, int clipSize, float reloadTime, float fireRate, int model,
            float falloffRange, float falloffAmount, float headshotScale, float spread) {
            this.falloffRange = falloffRange;
            this.falloffAmount = falloffAmount;
            this.headshotScale = headshotScale;
            this.spread = spread;
            weapon = new Weapon (damage, clipSize, reloadTime, fireRate, model, Weapon.Type.Auto);
        }
    }

    public struct SniperWeapon {
        public float falloffRange, falloffAmount, headshotScale;
        public Weapon weapon;

        public SniperWeapon (Damage damage, int clipSize, float reloadTime, float fireRate, int model,
            float falloffRange, float falloffAmount, float headshotScale) {
            this.falloffRange = falloffRange;
            this.falloffAmount = falloffAmount;
            this.headshotScale = headshotScale;
            weapon = new Weapon (damage, clipSize, reloadTime, fireRate, model, Weapon.Type.Sniper);
        }
    }

    public struct ScatterWeapon {
        public float falloffRange, falloffAmount, spread;
        public int count;
        public Weapon weapon;

        public ScatterWeapon (Damage damage, int clipSize, float reloadTime, float fireRate, int model,
            float falloffRange, float falloffAmount, float spread, int count) {
            this.falloffRange = falloffRange;
            this.falloffAmount = falloffAmount;
            this.spread = spread;
            this.count = count; 
            weapon = new Weapon (damage, clipSize, reloadTime, fireRate, model, Weapon.Type.Scatter);
        }
    }

    public struct Damage {
        public float amount, dotTime, dotAmount, stunTime, slowTime, slowAmount, rootTime, knockbackAmount;

        public Damage (float amount, float dotTime, float dotAmount, float stunTime, float slowTime, float slowAmount, float rootTime, float knockbackAmount) {
            this.amount = amount;
            this.dotTime = dotTime;
            this.dotAmount = dotAmount;
            this.stunTime = stunTime;
            this.slowTime = slowTime;
            this.slowAmount = slowAmount;
            this.rootTime = rootTime;
            this.knockbackAmount = knockbackAmount;
        }
    }

}