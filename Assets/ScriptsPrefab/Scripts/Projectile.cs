using UnityEngine;
using System.Collections;

public enum ProjectileType
{
	Standard,
	Seeker,
	ClusterBomb
}
public enum DamageType
{
	Direct,
	Explosion
}

public class Projectile : MonoBehaviour
{
	public ProjectileType projectileType = ProjectileType.Standard;
	public DamageType damageType = DamageType.Direct;
	public float damage = 100.0f;
	public float speed = 10.0f;
	public float initialForce = 1000.0f;
	public float lifetime = 30.0f;

	public float seekRate = 1.0f;
	public string seekTag = "Enemy";
	public GameObject explosion;
	public float targetListUpdateRate = 1.0f;

	public GameObject clusterBomb;
	public int clusterBombNum = 6;

	public int weaponType = 0;

	private float lifeTimer = 0.0f;
	private float targetListUpdateTimer = 0.0f;
	private GameObject[] enemyList;

	void Start()
	{
		UpdateEnemyList();
		GetComponent<Rigidbody>().AddRelativeForce(0, 0, initialForce);
	}

	void Update()
	{
		lifeTimer += Time.deltaTime;

		if (lifeTimer >= lifetime)
		{
			Explode(transform.position);
		}

		if (initialForce == 0)
			GetComponent<Rigidbody>().velocity = transform.forward * speed;

		if (projectileType == ProjectileType.Seeker)
		{
			targetListUpdateTimer += Time.deltaTime;

			if (targetListUpdateTimer >= targetListUpdateRate)
			{
				UpdateEnemyList();
			}

			if (enemyList != null)
			{
				float greatestDotSoFar = -1.0f;
				Vector3 target = transform.forward * 1000;
				foreach (GameObject enemy in enemyList)
				{
					if (enemy != null)
					{
						Vector3 direction = enemy.transform.position - transform.position;
						float dot = Vector3.Dot(direction.normalized, transform.forward);
						if (dot > greatestDotSoFar)
						{
							target = enemy.transform.position;
							greatestDotSoFar = dot;
						}
					}
				}

				Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * seekRate);
			}
		}
	}

	void UpdateEnemyList()
	{
		enemyList = GameObject.FindGameObjectsWithTag(seekTag);
		targetListUpdateTimer = 0.0f;
	}

	void OnCollisionEnter(Collision col)
	{
		Hit(col);
	}

	void Hit(Collision col)
	{
		Explode(col.contacts[0].point);

		if (damageType == DamageType.Direct)
		{
			col.collider.gameObject.SendMessageUpwards("ChangeHealth", -damage, SendMessageOptions.DontRequireReceiver);

			if (col.collider.gameObject.layer == LayerMask.NameToLayer("Limb"))
			{
				Vector3 directionShot = col.collider.transform.position - transform.position;
			}
		}
	}

	void Explode(Vector3 position)
	{
		if (explosion != null)
		{
			Instantiate(explosion, position, Quaternion.identity);
		}

		if (projectileType == ProjectileType.ClusterBomb)
		{
			if (clusterBomb != null)
			{
				for (int i = 0; i <= clusterBombNum; i++)
				{
					Instantiate(clusterBomb, transform.position, transform.rotation);
				}
			}
		}

		// Destroy the projectile
		Destroy(gameObject);

	}
	public void MultiplyDamage(float amount)
	{
		damage *= amount;
	}

	public void MultiplyInitialForce(float amount)
	{
		initialForce *= amount;
	}
}