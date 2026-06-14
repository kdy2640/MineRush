using System.Collections.Generic;
using UnityEngine;

public class StatCalculator
{
    // 계산 로직을 UpgradeManager에서 분리한 이유:
    // 업그레이드 구매 로직과 스탯 계산 로직의 책임을 나누기 위함이다.
    // 이후 장비, 버프, 합/곱 적용 순서 같은 계산 규칙이 늘어나면
    // 이 클래스에서만 계산식을 수정하도록 한다.
    public RuntimeStat Calculate(List<UpgradeState> upgradeStates)
    {
        RuntimeStat calculatedStat = new RuntimeStat();

        if (upgradeStates == null)
        {
            return calculatedStat;
        }

        foreach (UpgradeState state in upgradeStates)
        {
            if (state?.data?.statModifiers == null)
            {
                continue;
            }

            foreach (StatModifier modifier in state.data.statModifiers)
            {
                calculatedStat.Apply(modifier, state.level);
            }
        }

        return calculatedStat;
    }
}
