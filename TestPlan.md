# High-Level Test Plan for BinanceService

## 1. Setup and Initialization Tests
- [ Done ] Ensure the service is correctly initialized with the required dependencies.
- [ Done ] Verify the default cache duration is set from the configuration when not provided.

## 2. Caching Behavior Tests
- [ Done ] Confirm that each "Get" method (e.g., `GetBalanceAsync`, `GetAccountTradesAsync`) returns the cached result if available.
- [Done  ] Validate that when the cache is empty or expired, the service fetches fresh data from the API client and stores it in the cache.

## 3. Balance Retrieval Tests
- [ ] Test `GetBalanceAsync` to ensure it retrieves the correct balance when the cache is empty.
- [ ] Verify the returned balance matches the expected USDT Cross Wallet balance from the Binance API response.
- [ ] Confirm the returned balance is cached for subsequent calls.

## 4. Trades Retrieval Tests
- [ ] Test `GetAccountTradesAsync` to ensure it returns cached results if present.
- [ ] Validate that fresh trade data is correctly parsed from JSON, converted into DTOs, and cached.
- [ ] Check scenarios where no trades exist in the external API response.

## 5. Income History Tests
- [ ] Test `GetIncomeHistoryAsync` for caching.
- [ ] Validate that the income data is parsed into the correct fields and data types (e.g., `float`, `long` for IDs).
- [ ] Confirm correct handling of edge cases, like `null` or empty income fields.

## 6. Open Positions Tests
- [ ] Test `GetOpenPositionsAsync` to confirm caching behavior.
- [ ] Verify filtering logic that excludes positions with a zero amount.
- [ ] Check correct mapping of position details (entry price, unrealized profit, liquidation price, etc.).

## 7. Open Orders Tests
- [ ] Test `GetOpenOrdersAsync` to confirm correct caching.
- [ ] Verify the conversion logic for Buy/Sell plus `PositionSide` to the “OPEN LONG”, “CLOSE SHORT”, etc. strings.
- [ ] Confirm that the correct prices (price vs. stop price) are used.

## 8. Position History Tests
- [ ] Test `GetPositionHistoryAsync` end-to-end:
  - Confirm the method retrieves trades from both the API and repository.
  - Validate that combined trades are made unique.
  - Ensure the `ProcessTrades` method correctly identifies and groups positions for LONG and SHORT.
  - Confirm that closed positions have correct open/close times, average entry/exit prices, and PnL calculations.

## 9. Balance Snapshot Tests
- [ ] Test `GetBalanceSnapshotAsync` to confirm the method retrieves snapshots from the repository.
- [ ] Verify it updates the latest snapshot with the current balance from Binance.

## 10. Daily PNL Tests
- [ ] Test `GetDailyPNLAsync` to ensure data from the repository is fetched and combined with today’s income from the Binance income history.
- [ ] Validate correctness of daily PnL calculations based on real-time data for the current day.

## 11. Monthly Summary Tests
- [ ] Test `GetMonthlySummaryAsync` to confirm it correctly aggregates daily PNL by month.
- [ ] Verify edge cases, like transition between months or missing daily data.

## 12. History Tests
- [ ] Test `GetHistoryAsync` to ensure it filters data from the income history within the last X days.
- [ ] Validate grouping logic that splits data by day and sums up commissions and realized PnL.
- [ ] Confirm any multiplier logic is applied correctly.

## 13. Last Updated Time Tests
- [ ] Test `GetLastUpdatedTime` to ensure it returns the cached last-updated time or the current time if none is cached.

## 14. Error Handling Tests
- [ ] Validate behavior when the API client returns an error or invalid JSON.
- [ ] Confirm how the service behaves if the repository returns an empty set of data or if null fields appear in the database.
- [ ] Check how the service handles partial failures or exceptions during caching operations.

## 15. Performance and Reliability Tests
- [ ] Measure how the caching reduces repeated calls to the Binance API.
- [ ] Confirm the system remains responsive under a scenario of multiple parallel requests.

## 16. Integration Tests
- [ ] Integrate with a mock or test double for the Binance API client and repository to simulate various scenarios.
- [ ] Ensure each service method behaves correctly when dependent services return different combinations of data.

---

## Done
- [ ] (Check this box if all test plan items have been addressed)
