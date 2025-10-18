# Test Case Template

## Test Case Information

**Test Case ID:** TC-[NUMBER]  
**Test Case Name:** [Descriptive name of the test case]  
**Module/Feature:** [e.g., Authentication, Branch Management, Reservations]  
**Created By:** [Your Name]  
**Creation Date:** [YYYY-MM-DD]  
**Last Updated:** [YYYY-MM-DD]  
**Version:** [e.g., 1.0]  

---

## Test Case Details

**Priority:** [ ] Critical  [ ] High  [ ] Medium  [ ] Low  
**Test Type:** [ ] Functional  [ ] Integration  [ ] UI/UX  [ ] Security  [ ] Performance  
**Automation Status:** [ ] Manual  [ ] Automated  [ ] Partially Automated  

---

## Objective

**Purpose:**
[Clear statement of what this test case is designed to verify]

**Example:**
- Verify that users can successfully log in with valid credentials
- Ensure that branch creation validates all required fields
- Confirm that reservations cannot overlap for the same room

---

## Prerequisites

**System State:**
- [Required system configuration]
- [e.g., API must be running on port 57679]
- [e.g., Database must be initialized with seed data]

**Test Data:**
- [Required test data]
- [e.g., Valid user account: super@innhotel.com]
- [e.g., At least one branch must exist]

**User Permissions:**
- [Required user role or permissions]
- [e.g., User must be logged in as SuperAdmin]

**Environment:**
- [ ] Development
- [ ] Staging
- [ ] Production

---

## Test Steps

| Step # | Action | Expected Result |
|--------|--------|-----------------|
| 1 | [Detailed action to perform] | [What should happen] |
| 2 | [Next action] | [Expected outcome] |
| 3 | [Continue...] | [Expected result...] |

**Detailed Steps:**

### Step 1: [Action Description]
**Action:**
[Detailed description of what to do]

**Expected Result:**
[Detailed description of expected outcome]

**Actual Result:** [To be filled during test execution]

**Status:** [ ] Pass  [ ] Fail  [ ] Blocked  [ ] Skipped

---

### Step 2: [Action Description]
**Action:**
[Detailed description of what to do]

**Expected Result:**
[Detailed description of expected outcome]

**Actual Result:** [To be filled during test execution]

**Status:** [ ] Pass  [ ] Fail  [ ] Blocked  [ ] Skipped

---

### Step 3: [Action Description]
**Action:**
[Detailed description of what to do]

**Expected Result:**
[Detailed description of expected outcome]

**Actual Result:** [To be filled during test execution]

**Status:** [ ] Pass  [ ] Fail  [ ] Blocked  [ ] Skipped

---

## Test Data

**Input Data:**
```json
{
  "field1": "value1",
  "field2": "value2",
  "field3": "value3"
}
```

**Expected Output:**
```json
{
  "status": "success",
  "data": {
    "id": 1,
    "field1": "value1"
  }
}
```

---

## Validation Points

**Functional Validation:**
- [ ] Correct data is saved to database
- [ ] Appropriate success message is displayed
- [ ] User is redirected to correct page
- [ ] All required fields are validated

**UI Validation:**
- [ ] All elements are visible and properly aligned
- [ ] Loading indicators display during processing
- [ ] Error messages are clear and user-friendly
- [ ] Forms validate input correctly

**Data Validation:**
- [ ] Data types are correct
- [ ] Data formats are valid
- [ ] Required fields are enforced
- [ ] Data constraints are respected

**Security Validation:**
- [ ] Authentication is required
- [ ] Authorization is enforced
- [ ] Sensitive data is protected
- [ ] Input is sanitized

---

## Expected Results

**Success Criteria:**
1. [First success criterion]
2. [Second success criterion]
3. [Third success criterion]

**Performance Criteria:**
- Response time: < [X] seconds
- Page load time: < [X] seconds
- No memory leaks
- No console errors

---

## Test Execution

**Executed By:** [Tester Name]  
**Execution Date:** [YYYY-MM-DD]  
**Execution Time:** [HH:MM]  
**Build/Version:** [Version number]  

**Overall Status:** [ ] Pass  [ ] Fail  [ ] Blocked  [ ] Skipped  

**Pass/Fail Criteria:**
- All steps must pass for overall PASS
- Any step failure results in overall FAIL
- Blocked if prerequisites cannot be met
- Skipped if not applicable to current test cycle

---

## Defects Found

**Defect 1:**
- **Bug ID:** BUG-[NUMBER]
- **Description:** [Brief description]
- **Severity:** [ ] Critical  [ ] High  [ ] Medium  [ ] Low
- **Status:** [New/In Progress/Fixed]

**Defect 2:**
- **Bug ID:** BUG-[NUMBER]
- **Description:** [Brief description]
- **Severity:** [ ] Critical  [ ] High  [ ] Medium  [ ] Low
- **Status:** [New/In Progress/Fixed]

---

## Screenshots / Evidence

**Screenshot 1: [Description]**
![Screenshot 1](path/to/screenshot1.png)

**Screenshot 2: [Description]**
![Screenshot 2](path/to/screenshot2.png)

**Video Recording:** [Link if available]

---

## Notes / Comments

**Tester Notes:**
[Any observations, issues, or comments from the tester]

**Developer Notes:**
[Any notes from developers regarding this test case]

**Additional Information:**
[Any other relevant information]

---

## Test Case History

| Version | Date | Modified By | Changes Made |
|---------|------|-------------|--------------|
| 1.0 | [Date] | [Name] | Initial creation |
| 1.1 | [Date] | [Name] | Updated steps 2-3 |
| 1.2 | [Date] | [Name] | Added validation points |

---

## Related Test Cases

**Dependencies:**
- [TC-XXX: Test case that must run before this one]
- [TC-YYY: Related test case]

**Related Features:**
- [Feature/Module name]
- [Link to feature documentation]

---

## Automation Details (If Applicable)

**Automation Tool:** [e.g., Selenium, Cypress, Postman]  
**Script Location:** [Path to automation script]  
**Script Status:** [ ] Working  [ ] Needs Update  [ ] Broken  

**Automation Notes:**
[Notes about automation implementation]

---

## Sign-Off

**Reviewed By:** ________________  Date: __________  
**Approved By:** ________________  Date: __________  

---

**Last Updated:** [YYYY-MM-DD]