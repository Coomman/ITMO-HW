import java.util.concurrent.locks.ReentrantLock
import kotlin.concurrent.withLock

/**
 * Bank implementation.
 *
 * @author :Rauba Maksim
 */
class BankImpl(n: Int) : Bank {
    private val accounts: Array<Account> = Array(n) { Account() }

    override val numberOfAccounts: Int
        get() = accounts.size

    override fun getAmount(index: Int): Long
    {
        return accounts[index].amount
    }

    override val totalAmount: Long
        get() {
            var totalSum : Long = 0

            for (account in accounts){
                account.lock()
                totalSum += account.amount
            }

            for (account in accounts){
                account.unlock()
            }

            return totalSum
        }

    override fun deposit(index: Int, amount: Long): Long
    {
        require(amount > 0) { "Invalid amount: $amount" }

        val account = accounts[index]

        account.lock.withLock {
            check(!(amount > Bank.MAX_AMOUNT || account.amount + amount > Bank.MAX_AMOUNT)) { "Overflow" }

            account.amount += amount
            return account.amount
        }
    }

    override fun withdraw(index: Int, amount: Long): Long
    {
        require(amount > 0) { "Invalid amount: $amount" }

        val account = accounts[index]

        account.lock.withLock {
            check(account.amount - amount >= 0) { "Underflow" }

            account.amount -= amount
            return account.amount
        }
    }

    override fun transfer(fromIndex: Int, toIndex: Int, amount: Long)
    {
        require(amount > 0) { "Invalid amount: $amount" }
        require(fromIndex != toIndex) { "fromIndex == toIndex" }

        accounts[fromIndex.coerceAtMost(toIndex)].lock.withLock {
            accounts[fromIndex.coerceAtLeast(toIndex)].lock.withLock {
                val from = accounts[fromIndex]
                val to = accounts[toIndex]

                check(amount <= from.amount) { "Underflow" }
                check(!(amount > Bank.MAX_AMOUNT || to.amount + amount > Bank.MAX_AMOUNT)) { "Overflow" }

                from.amount -= amount
                to.amount += amount
            }
        }

    }

    /**
     * Private account data structure.
     */
    class Account {
        /**
         * Amount of funds in this account.
         */
        @Volatile
        var amount: Long = 0

        val lock: ReentrantLock = ReentrantLock()

        fun lock() {
            lock.lock()
        }
        fun unlock(){
            lock.unlock()
        }
    }
}