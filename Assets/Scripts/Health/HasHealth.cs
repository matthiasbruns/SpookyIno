interface HasHealth {
    bool CanBeDamaged{get;}    
    void ApplyDamage(int damage);
    int Health{get;}
}