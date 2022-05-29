/**
 * В теле класса решения разрешено использовать только переменные делегированные в класс RegularInt.
 * Нельзя volatile, нельзя другие типы, нельзя блокировки, нельзя лазить в глобальные переменные.
 *
 * @author : Rauba Maksim
 */

class Solution : MonotonicClock
{
    private var c1_hours by RegularInt(0)
    private var c1_minutes by RegularInt(0)
    private var c1_seconds by RegularInt(0)
    private var c2_hours by RegularInt(0)
    private var c2_minutes by RegularInt(0)
    private var c2_seconds by RegularInt(0)

    override fun write(time: Time)
    {
        c2_hours = time.d1
        c2_minutes = time.d2
        c2_seconds = time.d3

        c1_seconds = c2_seconds
        c1_minutes = c2_minutes
        c1_hours = c2_hours
    }

    override fun read(): Time
    {
        val r1 = Time(c1_hours, c1_minutes, c1_seconds)

        var r2 = Time(c2_seconds, c2_minutes, c2_hours)
        r2 = r2.copy(d1 = r2.d3, d2 = r2.d2, d3 = r2.d1)

        if (r1.compareTo(r2) == 0)
            return r1

        if (r1.d1 == r2.d1)
        {
            if (r1.d2 == r2.d2)
                return r2

            return Time(r2.d1, r2.d2, 0)
        }

        return Time(r2.d1, 0, 0)
    }
}