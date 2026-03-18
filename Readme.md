# Demo: Debugging .NET with GitHub Copilot

# 15-Minute Demo Script

---

## Setup Checklist *(before the room fills up)*

- [ ]  Remember to talk slow. This is old-ish to you, but new-ish to some others, and English also isn’t a first language for many in attendance.
- [ ]  VS 2022 17.8+ open with `OrderProcessor` solution loaded
- [ ]  GitHub Copilot signed in and active (check the status bar icon)
- [ ]  Ensure GPT-4.1 is selected as the model
- [ ]  Ensure unpinned “order” properties prior to demoing
- [ ]  App in a **broken** starting state — do NOT pre-fix any bugs
- [ ]  **Font size:** Tools → Options → Environment → Fonts and Colors → in the "Show settings for" dropdown, select **Environment** → increase the Size
- [ ]  **Monitor resolution:** set to **1920 × 1080**
- [ ]  **Windows Display Settings:** Scale and layout → **125% or 150%**
- [ ]  **Theme:** try a High Contrast theme for visibility
- [ ]  **Share the Window, not the Screen** — and confirm terminal output is visible within that window
- [ ]  Copilot Chat panel closed at start (the contextual pop-ups land harder when they appear organically)

---

## Scenario Map

| Slot | Scenario | Why it's in |
| --- | --- | --- |
| 1 | **Exception Helper** | Highest wow-per-minute. The fix emerges right where the crash is. |
| 2 | **Conditional Breakpoints + DataTips** | Immediately practical. Every dev can use this tomorrow. |
| 3 | **IEnumerable Visualizer + LINQ** | Visually striking — hard to appreciate without seeing it live. |
| 4 | **Inline Values + Explain Unexpected Output** | Natural payoff: "I see a wrong number, now what?" |

Scenarios #6 (repo context), #7 (Parallel Stacks deadlocks), #8 (Debugger Agent for tests), and #9 (Profiler Agent) are mentioned verbally at the close as candidates for follow-up sessions.

---

## Timing & Script

### [0:00 – 1:00] Intro

> Today I’m going to present how to use the “Debug(ger)” agent in VS 2022 & VS 2026. There are some agents/skills that aren’t currently “in the box” in VS 2022, but are automatically available when you open VS 2026.

Similar to Claude Skills (which, btw, are also supported by Copilot now), these specialized agents *should* automatically “kick in” when they’re needed. Alternatively, you can select them explicitly.

First, let’s look at the specialized agents available in VS 2026.
> 

## GitHub Copilot Extension Agents

- **Copilot Coding Agent:** This is your primary partner for writing and refactoring code. It has the best context of your entire workspace and is optimized for generating boilerplate, suggesting logic, or explaining complex functions.
- **Debugger:** This agent helps you diagnose why your code isn't behaving. You can ask it to explain call stacks, suggest why a variable has a specific value, or provide fixes for identified exceptions.
- **Modernize:** Highly useful for legacy projects, this agent specializes in upgrading syntax (e.g., moving from older .NET versions to the latest C# features) or migrating libraries to more modern alternatives.
- **Profiler:** This is for performance tuning. It helps interpret profiling data, identifying "hot paths" or memory leaks, and suggesting optimizations to reduce CPU or RAM usage.
- **Test:** Focused on quality assurance, this agent can generate unit tests (using frameworks like xUnit or MSTest), suggest edge cases you might have missed, and help fix failing tests.
- **VS (Visual Studio):** This is a meta-agent for the IDE itself. Use it to ask how to find specific settings, how to use hidden VS features, or to perform IDE-level commands through natural language.
- **Custom Agent(s)…**

> Moving on to the code, this is a simple, small app I vibe-coded just the other day specifically for this demo. I’ll drop a link to the repo at the end of this demo.
> 

---

### [1:00 – 4:00] Segment 1 — Exception Helper *(3 min)*

**What you're showing:** Copilot understands exceptions in context, proposes a targeted fix, and lets you verify it before applying.

**Steps:**

1. Press **F5**. The app crashes immediately with a `NullReferenceException`.
2. The **Exception Helper** window appears. Point it out.
    
    > "Normally this tells you *what* blew up. But now there's an **Analyze with Copilot** button."
    > 
3. Click **Analyze with Copilot**.
4. The Chat panel opens — exception type, offending line, and call stack are already loaded as context.
    
    > "Notice I didn't type a thing. Copilot already knows the exception, the line, and what's in scope."
    > 
5. Copilot (GPT-4.1) responds with: *"Would you like me to update Bob's order to use an empty list for Items?"*
6. Type **"yes"**. Copilot generates the diff.
    
    > "I can review exactly what it's changing before I accept anything."
    > 
7. Apply the fix. Rerun. The crash is gone.
8. Now ask in the chat: **"Are there other places where Items could be null?"** Show the response.
    
    > Copilot scans the visible code and flags any other potential null-access paths.
    > 
9. Now ask (w**ith GPT-4.1)**, *"How else can I fix the null list?"* Show the response.
    
    > We’re looking for a suggestion to initialize Items as an empty list instead of null.
    > 
10. Re-run this with GPT-4o model, and continue on with that.
    
    > "Notice how 4.1 asks for permission? That's a **conversational fallback**. It means the model didn't provide a valid code diff that the IDE could verify. 4o, however, is using **Agentic Workflows**—it provides a verifiable diff that the IDE recognizes, which is why we get that 'one-click' Apply button."
    > 

**Talking points:**

- Copilot doesn't just identify the null — it traces *why* it's null (Bob's order was never given an item list).
- In a codebase with repo context enabled, that follow-up question searches across the whole project, not just the open file.

---

### [4:00 – 7:00] Segment 2 — Conditional Breakpoints + DataTips *(3 min)*

**What you're showing:** Copilot suggests correct, semantically relevant breakpoint conditions. DataTips let you pin properties for instant visibility on future pauses.

**Steps:**

1. Press **Ctrl+G**, type **33**, press Enter to jump to the `foreach (var order in orders)` loop.
2. Right-click in the gutter *inside the loop body* → **Insert Temporary Breakpoint**.
3. Right-click the breakpoint and select “Conditions…”.
4. Click into the condition field and pause — let Copilot's suggestions appear.
    
    > "Copilot is reading the `Order` type and its properties and generating conditions that actually make sense for *this* loop."
    > 
5. Choose a suggestion targeting Carol: `order.CustomerName == "Carol White"` or `order.Id == 1003`.
6. Press **F5**. The debugger breaks only on Carol's iteration.
    
    > "Five orders in the list. We landed exactly on the one we wanted."
    > 
7. Hover over the `order` variable — a **DataTip** appears. Expand it to show properties.
    
    > "These are DataTips. I can **pin** individual properties by clicking the pin icon — pinned values then float in the editor margin on future pauses, so I don't have to expand the tooltip every time."
    Pin one or two properties (e.g., `CustomerName`, `Items.Count`) to demonstrate.
    > 
8. Create a tracepoint:
`$FUNCTION: {order.CustomerName} has {order.Items.Count} items`

**Talking points:**

- The real value scales with loop size — hundreds of items, complex filter criteria, and you still land in one shot.

---

### [7:00 – 10:30] Segment 3 — IEnumerable Visualizer + LINQ Analysis *(3.5 min)*

**What you're showing:** Copilot can inspect a LINQ result set mid-execution and explain why the data looks wrong.

**Steps:**

1. Press **Ctrl+G**, type **24**, press Enter to jump to the `highValueOrders` LINQ query.
2. Set a breakpoint on the line *immediately after* the assignment.
3. Press **F5**, hit the breakpoint.
4. Hover over `highValueOrders` — the DataTip appears with a magnifying glass icon. Click it to open the **IEnumerable Visualizer**.
    
    > "This is the tabular visualizer. I can see every row in the result set right here — no Watch expressions, no `.ToList()` hacks."
    > 
5. The table shows only **1 order**.
    
    > "We'd expect 2 or 3 orders over $100. Something's off."
    > 
6. Open the Copilot Chat panel and type: **"We'd expect 2–3 orders over $100, something's off."**
7. Copilot identifies the bug: `Sum(item => item.Price)` ignores `Quantity` — a 3× item still counts as 1×.
8. Copilot responds with: *"Would you like me to update the filter to use price × quantity?"* Type **"yes"** — or use the **Apply** button if it appears inline in the response.
9. Rerun. The visualizer now shows **3 orders**.
10. Disable the breakpoint.

**Talking points:**

- Works for any `IEnumerable<T>`: EF Core query results, in-memory collections, custom iterators.
- The chat prompt here was intentionally vague — Copilot had enough context from the paused state to know which query to look at.
- Disabling vs. deleting breakpoints is good practice during a demo: disabled breakpoints stay visible in the gutter as a record of where you've been.

---

### [10:30 – 13:30] Segment 4 — Inline Values + Explaining Unexpected Output *(3 min)*

**What you're showing:** Copilot can look at a live variable value that's wrong and reason about why the logic produced it.

**Steps:**

1. Press **Ctrl+G**, type **55**, press Enter to jump to the discount logic inside `ProcessOrder`.
2. Set a breakpoint on the first `Console.WriteLine` line.
3. Press **F5**. Use the conditional breakpoint from Segment 2 (still disabled, re-enable it, or step past earlier orders) to land on Carol's order.
4. Right-click the “total” variable in Locals and **Analyze with Copilot**
5. **Inline values** appear in the editor margin: `subtotal = $249.99`, `discount = $0.00`.
    
    > "Carol has a $250 order. Zero discount. That doesn't feel right."
    > 
6. In the Copilot Chat panel, type: **"A $250 order should receive a discount."**
7. Copilot identifies the `> 500` threshold and explains that the business rule (per the code comments) should be `> 50`.
8. Copilot asks for confirmation. Reply **"yes"**. Apply the fix.
9. Rerun. Carol now receives a `$24.99` discount.
10. Disable the breakpoint.

**Talking points:**

- The **Autos/Locals → right-click → Ask Copilot** path is an alternative to typing in the chat panel — Copilot gets the variable name and value automatically.
- Inline return values extend this further: if `subtotal` came from a multi-step method chain, each intermediate return value appears on the same line in the margin.

---

### [13:30 – 15:00] Close *(1.5 min)*

> "With a professional license, Copilot supports more advanced models than GPT 4, and the quality and depth of the analysis scales with the model. Worth experimenting with if you want richer explanations."
> 

> Here’s a link to a much more in-depth video produced by Microsoft that covers this and more. It's a year old, but still very relevant.
> 
📎 **Post in Teams chat:** https://www.youtube.com/watch?v=iFjQghRbJUw

> Here’s the link to this repo.
> 
📎 **Post in Teams chat:** [link to this repo]

> and here’s a couple links to the current official documentation on this topic.
> 
📎 **Post in Teams chat:** https://learn.microsoft.com/en-us/visualstudio/debugger/debug-with-copilot?view=visualstudio

📎 **Post in Teams chat:** https://learn.microsoft.com/en-us/visualstudio/releases/2026/release-notes#debugging--diagnostics

**Mention as future sessions:**

- **Debugger Agent (#8):** Point Copilot at a failing unit test and it runs the debugger autonomously — inspects state, proposes a fix, no manual stepping required.
- **Parallel Stacks + AI (#7):** Deadlock and hang diagnosis — Copilot analyzes all threads' call stacks simultaneously.
- **Profiler Agent (#9):** "Why is this slow?" — Copilot-assisted perf analysis without reading flame graphs manually.
- **Repo Context in Exception Helper (#6):** With full repo access, Copilot can explain *business-logic* exceptions, not just language-level ones.

### **Questions?**

---

## Contingency Notes

| If... | Then... |
| --- | --- |
| Copilot is slow to respond | Narrate what you'd expect while it loads — the analysis is almost always correct |
| Conditional breakpoint suggestions don't appear | Type `order.` manually; autocomplete kicks in — pivot to "same variable awareness" |
| Chat panel doesn't auto-populate exception context | Paste the exception message and say "full repo context makes this even richer" |
| Running short on time | Cut Segment 4 — the first three are the strongest arc |
| "Analyze with Copilot" missing in a spot | Describe what it would say, open chat manually, ask the question — same result |