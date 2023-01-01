public enum ProjectileTypeEnum
{
    // Target 기반
        // 가장 기본적인 형태
        Bullet,
        // 폭발형:: 도달 시 주변에 스플레시 데미지?
        Explosive,

    // Non-Target 기반
        // 몬스터 추적용 
        Aura,
        // 도달 후 해당 위치에서 범위 (일종의 설치)
        Area,
}