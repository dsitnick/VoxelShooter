using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData {

    public struct GameData {
        public List<CharacterData> characters;
        public List<WeaponData> weapons;
    }

    public struct CharacterData {
        private const int WEAPON_COUNT = 4;

        public float healthMax, shieldMax, shieldRegen, shieldDelay;
        public float speed, jumpHeight;

        public int headModel, bodyModel, handModel, footModel;
        public int[] weapons;

        public CharacterData (float healthMax, float shieldMax, float shieldRegen, float shieldDelay, float speed, float jumpHeight,
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

		public static CharacterData Default = new CharacterData (100, 100, 20, 1.5f, 8, 6, -1, -1, -1, -1, new int[] { -1, -1, -1, -1 });

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

	public abstract class WeaponData {
		public enum Type { Sniper };

		public Damage damage;
		public float rate;
		public int model;
		public Type type;

		public WeaponData (Damage damage, float rate, int model, Type type){
			this.damage = damage;
			this.rate = rate;
			this.model = model;
			this.type = type;
		}
	}

	public abstract class RangedWeaponData : WeaponData {
		public int clip, ammo;
		public float reload;

		public RangedWeaponData(Damage damage, float rate, int model, Type type, int clip, int ammo, float reload)
			: base (damage, rate, model, type){
			this.clip = clip;
			this.ammo = ammo;
			this.reload = reload;
		}
	}

	public abstract class HitscanWeaponData : RangedWeaponData {
		public float falloffStart, falloffEnd;

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

		public Damage(float amount){
			this.amount = amount;
			dotTime = dotAmount = stunTime = slowTime = slowAmount = rootTime = knockbackAmount = 0;
		}
    }

}