using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SkillExecutor
{

    void executeSingle(Transform caster, Transform target);
    void executeMultiple(Transform caster, Transform[] targets);
}