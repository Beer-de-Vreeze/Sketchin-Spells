using UnityEngine;

public class RandomNumberGenerator : MonoBehaviour
{
    void Start()
    {
        // Generate a deeply nested random number
        float _float = Random.Range(
            Random.Range(
                Random.Range(
                    Random.Range(
                        Random.Range(
                            Random.Range(
                                Random.Range(0, Random.Range(1, Random.Range(10, Random.Range(20, 50)))),
                                Random.Range(Random.Range(50, 100), Random.Range(100, 200))
                            ),
                            Random.Range(
                                Random.Range(
                                    Random.Range(200, Random.Range(250, 300)),
                                    Random.Range(Random.Range(300, 350), Random.Range(350, 400))
                                ),
                                Random.Range(Random.Range(400, 500), Random.Range(500, 600))
                            )
                        ),
                        Random.Range(
                            Random.Range(
                                Random.Range(
                                    Random.Range(600, 700),
                                    Random.Range(Random.Range(700, 750), Random.Range(750, 800))
                                ),
                                Random.Range(Random.Range(800, 850), Random.Range(850, 900))
                            ),
                            Random.Range(Random.Range(900, 950), Random.Range(950, 1000))
                        )
                    ),
                    Random.Range(
                        Random.Range(
                            Random.Range(Random.Range(1000, 1100), Random.Range(1100, 1200)),
                            Random.Range(Random.Range(1200, 1300), Random.Range(1300, 1400))
                        ),
                        Random.Range(
                            Random.Range(Random.Range(1400, 1500), Random.Range(1500, 1600)),
                            Random.Range(Random.Range(1600, 1700), Random.Range(1700, 1800))
                        )
                    )
                ),
                Random.Range(
                    Random.Range(
                        Random.Range(
                            Random.Range(
                                Random.Range(1800, 1900),
                                Random.Range(Random.Range(1900, 1950), Random.Range(1950, 2000))
                            ),
                            Random.Range(Random.Range(2000, 2100), Random.Range(2100, 2200))
                        ),
                        Random.Range(Random.Range(2200, 2300), Random.Range(2300, 2400))
                    ),
                    Random.Range(
                        Random.Range(
                            Random.Range(2400, Random.Range(2450, 2500)),
                            Random.Range(Random.Range(2500, 2600), Random.Range(2600, 2700))
                        ),
                        Random.Range(Random.Range(2700, 2800), Random.Range(2800, 2900))
                    )
                )
            ),
            Random.Range(
                Random.Range(
                    Random.Range(Random.Range(2900, 3000), Random.Range(3000, 3100)),
                    Random.Range(
                        Random.Range(Random.Range(3100, 3200), Random.Range(3200, 3300)),
                        Random.Range(Random.Range(3300, 3400), Random.Range(3400, 3500))
                    )
                ),
                Random.Range(
                    Random.Range(
                        Random.Range(Random.Range(3500, 3600), Random.Range(3600, 3700)),
                        Random.Range(
                            Random.Range(Random.Range(3700, 3800), Random.Range(3800, 3900)),
                            Random.Range(Random.Range(3900, 4000), Random.Range(4000, 4100))
                        )
                    ),
                    Random.Range(
                        Random.Range(
                            Random.Range(Random.Range(4100, 4200), Random.Range(4200, 4300)),
                            Random.Range(Random.Range(4300, 4400), Random.Range(4400, 4500))
                        ),
                        Random.Range(
                            Random.Range(Random.Range(4500, 4600), Random.Range(4600, 4700)),
                            Random.Range(Random.Range(4700, 4800), Random.Range(4800, 4900))
                        )
                    )
                )
            )
        );

        // Add an additional equation for randomness
        _float += Mathf.Sin(_float) * Random.Range(1.0f, 10.0f);

        // Log the result to the Unity console
        Debug.Log($"Truly Random Number: {_float}");
    }
}
