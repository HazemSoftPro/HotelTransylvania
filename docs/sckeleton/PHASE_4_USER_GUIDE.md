# Phase 4 User Guide: Payment Processing, Notifications & Reporting

**InnHotel Hotel Management System**  
**Version:** 1.0  
**Date:** October 19, 2025

---

## Table of Contents

1. [Payment Processing](#payment-processing)
2. [Notification System](#notification-system)
3. [Reporting & Analytics](#reporting--analytics)
4. [Troubleshooting](#troubleshooting)
5. [Best Practices](#best-practices)

---

## Payment Processing

### Overview

The payment processing system allows you to securely accept payments from guests, process refunds, and maintain complete financial records.

### Accepting Payments

#### Step-by-Step Process

1. **Navigate to Reservation**
   - Open the reservation that requires payment
   - Click on the "Payments" tab

2. **Create New Payment**
   - Click "Add Payment" button
   - Enter payment amount
   - Select payment method:
     - Cash
     - Credit Card
     - Debit Card
     - Bank Transfer
     - Online Payment
     - Mobile Payment
     - Check

3. **Add Payment Details**
   - Enter description (optional)
   - Review payment information
   - Click "Process Payment"

4. **Confirmation**
   - System processes payment
   - Transaction ID is generated
   - Payment confirmation is displayed
   - Guest receives confirmation notification

#### Payment Methods Explained

**Cash**
- Direct cash payment at front desk
- Requires manual counting and verification
- Receipt should be provided to guest

**Credit/Debit Card**
- Processed through payment terminal
- Transaction ID automatically recorded
- Instant confirmation

**Bank Transfer**
- Guest transfers money to hotel account
- Verify transfer before confirming payment
- Record bank reference number

**Online Payment**
- Guest pays through online portal
- Automatic confirmation
- Instant processing

**Mobile Payment**
- Payment through mobile apps (Apple Pay, Google Pay, etc.)
- Quick and convenient
- Instant confirmation

**Check**
- Guest provides check
- Verify check details
- Record check number
- Allow clearing time before confirming reservation

### Viewing Payment History

1. **For a Specific Reservation**
   - Open reservation details
   - Click "Payments" tab
   - View all payments for this reservation

2. **All Payments**
   - Navigate to "Payments" section
   - View list of all payments
   - Use filters to find specific payments

### Filtering Payments

You can filter payments by:
- **Status:** Pending, Processing, Completed, Failed, Cancelled, Refunded
- **Date Range:** Select start and end dates
- **Reservation:** Filter by specific reservation
- **Payment Method:** Filter by payment type

### Processing Refunds

#### When to Issue Refunds

- Guest cancellation within refund policy
- Service issues or complaints
- Overbooking situations
- Billing errors

#### Refund Process

1. **Locate Payment**
   - Find the payment to refund
   - Click on payment details

2. **Initiate Refund**
   - Click "Process Refund" button
   - Enter refund amount:
     - Full refund: Enter total payment amount
     - Partial refund: Enter specific amount

3. **Provide Reason**
   - Enter reason for refund (required)
   - Examples:
     - "Guest cancellation - within policy"
     - "Service issue compensation"
     - "Billing error correction"

4. **Confirm Refund**
   - Review refund details
   - Click "Confirm Refund"
   - System processes refund
   - Guest receives refund notification

#### Refund Policies

- **Full Refund:** Entire payment amount returned
- **Partial Refund:** Portion of payment returned (e.g., cancellation fee deducted)
- **Processing Time:** Refunds typically process within 5-10 business days
- **Authorization:** Only Administrators and Managers can process refunds

### Payment Reports

Access payment reports through the Reporting section:
- Total payments received
- Payments by method
- Refund summary
- Net revenue

---

## Notification System

### Overview

The notification system automatically sends messages to guests and staff through multiple channels: email, SMS, in-app notifications, and push notifications.

### Notification Types

#### 1. Reservation Confirmation
- **When:** Immediately after reservation is created
- **Sent to:** Guest
- **Contains:** Reservation details, room number, check-in/check-out dates
- **Channels:** Email, In-App

#### 2. Check-In Reminder
- **When:** 24 hours before check-in date
- **Sent to:** Guest
- **Contains:** Check-in time, room number, required documents
- **Channels:** Email, SMS, Push

#### 3. Check-Out Reminder
- **When:** 24 hours before check-out date
- **Sent to:** Guest
- **Contains:** Check-out time, final charges, feedback request
- **Channels:** Email, SMS, Push

#### 4. Payment Received
- **When:** After successful payment processing
- **Sent to:** Guest
- **Contains:** Payment amount, transaction ID, receipt
- **Channels:** Email, In-App

#### 5. Payment Failed
- **When:** Payment processing fails
- **Sent to:** Guest and Staff
- **Contains:** Failure reason, alternative payment options
- **Channels:** Email, In-App

#### 6. Reservation Cancelled
- **When:** Reservation is cancelled
- **Sent to:** Guest
- **Contains:** Cancellation details, refund information
- **Channels:** Email, SMS

#### 7. Waitlist Room Available
- **When:** Room becomes available from waitlist
- **Sent to:** Guest on waitlist
- **Contains:** Room details, deadline to confirm
- **Channels:** Email, SMS, Push

### Managing Notification Preferences

#### For Guests

1. **Access Preferences**
   - Guest logs into their account
   - Navigates to "Notification Preferences"

2. **Configure Channels**
   - For each notification type, choose:
     - ✅ Email: Receive via email
     - ✅ SMS: Receive via text message
     - ✅ In-App: See in application
     - ✅ Push: Receive push notifications

3. **Save Preferences**
   - Click "Save Preferences"
   - Changes take effect immediately

#### For Staff

Staff can view notification delivery status:
- Navigate to "Notifications" section
- View sent notifications
- Check delivery status
- View failed notifications and retry

### Viewing Notification History

1. **Access Notification History**
   - Navigate to "Notifications" section
   - View list of all notifications

2. **Filter Notifications**
   - By user
   - By type
   - By status (Sent, Failed, Read)
   - By date range

3. **View Details**
   - Click on notification
   - See full message content
   - View delivery status
   - See read timestamp (if applicable)

### Troubleshooting Notifications

#### Notification Not Received

1. **Check Delivery Status**
   - View notification history
   - Check if marked as "Sent"
   - Look for error messages

2. **Verify Contact Information**
   - Ensure guest email is correct
   - Verify phone number for SMS
   - Check spam/junk folders

3. **Check Preferences**
   - Verify guest hasn't disabled this notification type
   - Ensure channel is enabled

#### Failed Notifications

- System automatically retries failed notifications
- Check error message for specific issue
- Common issues:
  - Invalid email address
  - Phone number format incorrect
  - Email server issues
  - SMS service unavailable

---

## Reporting & Analytics

### Overview

The reporting system provides comprehensive insights into hotel operations, revenue, and performance.

### Occupancy Reports

#### Generating Occupancy Report

1. **Navigate to Reports**
   - Click "Reports" in main menu
   - Select "Occupancy Report"

2. **Select Parameters**
   - **Date Range:** Choose start and end dates
   - **Branch:** Select specific branch or "All Branches"
   - Click "Generate Report"

3. **View Report**
   - Overall occupancy rate
   - Total rooms vs. occupied rooms
   - Check-ins and check-outs
   - Daily occupancy breakdown
   - Room type occupancy

#### Understanding Occupancy Metrics

**Occupancy Rate**
- Percentage of rooms occupied
- Formula: (Occupied Rooms / Total Rooms) × 100
- Example: 38 occupied out of 50 rooms = 76% occupancy

**RevPAR (Revenue Per Available Room)**
- Average revenue per room (occupied or not)
- Formula: Total Room Revenue / Total Available Rooms
- Helps measure overall performance

**Daily Breakdown**
- Shows occupancy for each day in date range
- Identifies peak and slow periods
- Helps with staffing and pricing decisions

**Room Type Breakdown**
- Occupancy by room category (Standard, Deluxe, Suite)
- Identifies popular room types
- Helps with inventory management

### Revenue Reports

#### Generating Revenue Report

1. **Navigate to Reports**
   - Click "Reports" in main menu
   - Select "Revenue Report"

2. **Select Parameters**
   - **Date Range:** Choose start and end dates
   - **Branch:** Select specific branch or "All Branches"
   - Click "Generate Report"

3. **View Report**
   - Total revenue
   - Room revenue vs. service revenue
   - Payment method breakdown
   - Daily revenue trends
   - Average revenue per reservation
   - Refunds and net revenue

#### Understanding Revenue Metrics

**Total Revenue**
- Sum of all payments received
- Includes room charges and services

**Net Revenue**
- Total revenue minus refunds
- Actual money retained by hotel

**Average Revenue Per Reservation**
- Total revenue divided by number of reservations
- Indicates average booking value

**Payment Method Breakdown**
- Shows revenue by payment type
- Helps understand guest payment preferences
- Useful for financial planning

**Daily Revenue Trends**
- Revenue for each day in date range
- Identifies high and low revenue days
- Helps with forecasting

### Employee Performance Reports

#### Generating Performance Report

1. **Navigate to Reports**
   - Click "Reports" in main menu
   - Select "Employee Performance Report"

2. **Select Parameters**
   - **Date Range:** Choose start and end dates
   - **Branch:** Select specific branch or "All Branches"
   - Click "Generate Report"

3. **View Report**
   - Reservations processed per employee
   - Check-ins and check-outs handled
   - Payments processed
   - Revenue generated
   - Guest satisfaction scores

#### Using Performance Data

**For Managers:**
- Identify top performers
- Recognize employee contributions
- Identify training needs
- Make staffing decisions

**For Employees:**
- Track personal performance
- Set improvement goals
- Understand contribution to hotel success

### Exporting Reports

#### Export to PDF

1. **Generate Report**
   - Create the report you want to export

2. **Export**
   - Click "Export to PDF" button
   - Report is formatted for printing
   - Save or print PDF

3. **Uses:**
   - Presentations to management
   - Board meetings
   - Stakeholder reports
   - Archival purposes

#### Export to Excel

1. **Generate Report**
   - Create the report you want to export

2. **Export**
   - Click "Export to Excel" button
   - Data exported in spreadsheet format

3. **Uses:**
   - Further analysis
   - Custom calculations
   - Integration with accounting software
   - Data manipulation

### Report Best Practices

#### Frequency

**Daily Reports:**
- Quick occupancy check
- Revenue snapshot
- Identify immediate issues

**Weekly Reports:**
- Trend analysis
- Performance review
- Staffing adjustments

**Monthly Reports:**
- Comprehensive analysis
- Financial reporting
- Strategic planning

**Quarterly/Annual Reports:**
- Long-term trends
- Year-over-year comparison
- Strategic decision making

#### Interpretation Tips

1. **Look for Trends**
   - Don't focus on single days
   - Identify patterns over time
   - Compare to previous periods

2. **Context Matters**
   - Consider seasonality
   - Account for special events
   - Factor in local conditions

3. **Action-Oriented**
   - Use data to make decisions
   - Identify opportunities
   - Address problems proactively

---

## Troubleshooting

### Payment Issues

#### Payment Processing Failed

**Symptoms:**
- Payment shows "Failed" status
- Error message displayed

**Solutions:**
1. Verify payment details are correct
2. Check payment method is valid
3. Ensure sufficient funds (for cards)
4. Retry payment
5. Try alternative payment method
6. Contact payment provider if issue persists

#### Refund Not Processing

**Symptoms:**
- Refund request fails
- Error message displayed

**Solutions:**
1. Verify original payment was completed
2. Ensure refund amount doesn't exceed original payment
3. Check authorization (only Managers/Administrators can refund)
4. Verify reason is provided
5. Contact technical support if issue persists

### Notification Issues

#### Notifications Not Sending

**Symptoms:**
- Notifications show "Failed" status
- Guests not receiving notifications

**Solutions:**
1. Verify guest contact information
2. Check notification preferences
3. Verify email/SMS service is configured
4. Check notification history for error messages
5. Retry failed notifications
6. Contact technical support if issue persists

#### Duplicate Notifications

**Symptoms:**
- Guest receives same notification multiple times

**Solutions:**
1. Check notification history
2. Verify no duplicate reservations
3. Ensure notification wasn't manually resent
4. Contact technical support if issue persists

### Reporting Issues

#### Report Not Generating

**Symptoms:**
- Report generation fails
- Error message displayed

**Solutions:**
1. Verify date range is valid
2. Ensure data exists for selected period
3. Try smaller date range
4. Refresh page and retry
5. Contact technical support if issue persists

#### Report Data Seems Incorrect

**Symptoms:**
- Numbers don't match expectations
- Missing data in report

**Solutions:**
1. Verify date range is correct
2. Check branch filter
3. Ensure all data has been entered in system
4. Compare with raw data
5. Contact technical support if discrepancy persists

---

## Best Practices

### Payment Processing

1. **Always Verify Payment Details**
   - Double-check amount
   - Confirm payment method
   - Verify guest information

2. **Provide Receipts**
   - Always provide payment confirmation
   - Keep copies for records
   - Ensure guest receives notification

3. **Handle Refunds Promptly**
   - Process refunds quickly
   - Communicate clearly with guest
   - Document reason thoroughly

4. **Maintain Records**
   - Keep complete payment history
   - Document all transactions
   - Regular reconciliation with accounting

### Notification Management

1. **Respect Guest Preferences**
   - Honor notification preferences
   - Don't override guest choices
   - Provide easy opt-out options

2. **Monitor Delivery**
   - Regularly check notification status
   - Address failed notifications promptly
   - Verify critical notifications are received

3. **Keep Templates Professional**
   - Use provided templates
   - Maintain consistent tone
   - Ensure accuracy of information

4. **Timely Communication**
   - Send notifications at appropriate times
   - Don't spam guests
   - Provide valuable information

### Reporting

1. **Regular Review**
   - Check reports daily/weekly
   - Look for trends and patterns
   - Take action on insights

2. **Data Accuracy**
   - Ensure all data is entered correctly
   - Verify information before generating reports
   - Cross-check with other sources

3. **Share Insights**
   - Distribute reports to relevant staff
   - Discuss findings in team meetings
   - Use data for decision making

4. **Archive Reports**
   - Save important reports
   - Maintain historical records
   - Use for year-over-year comparisons

---

## Getting Help

### Support Resources

**Documentation:**
- This user guide
- API documentation
- Technical implementation report

**Training:**
- Staff training sessions
- Video tutorials (if available)
- Hands-on practice sessions

**Technical Support:**
- Contact development team
- Submit support tickets
- Emergency support hotline

### Common Questions

**Q: How long does payment processing take?**  
A: Most payments process instantly. Bank transfers may take 1-3 business days to confirm.

**Q: Can I edit a notification after it's sent?**  
A: No, sent notifications cannot be edited. You can send a new notification if needed.

**Q: How far back can I generate reports?**  
A: You can generate reports for any date range where data exists in the system.

**Q: Who can process refunds?**  
A: Only users with Administrator or Manager roles can process refunds.

**Q: Can guests change their notification preferences?**  
A: Yes, guests can configure their preferences through their account settings.

---

## Appendix

### Payment Status Definitions

- **Pending:** Payment initiated but not yet processed
- **Processing:** Payment currently being processed
- **Completed:** Payment successfully processed
- **Failed:** Payment processing failed
- **Cancelled:** Payment was cancelled
- **Refunded:** Payment was fully refunded
- **PartiallyRefunded:** Payment was partially refunded

### Notification Status Definitions

- **Pending:** Notification created but not yet sent
- **Sent:** Notification successfully sent
- **Delivered:** Notification confirmed delivered (where applicable)
- **Read:** Notification was read by recipient
- **Failed:** Notification delivery failed

### Report Metrics Glossary

- **Occupancy Rate:** Percentage of rooms occupied
- **RevPAR:** Revenue Per Available Room
- **ADR:** Average Daily Rate
- **Total Revenue:** Sum of all payments
- **Net Revenue:** Total revenue minus refunds
- **Average Revenue Per Reservation:** Total revenue divided by reservations

---

**Document Version:** 1.0  
**Last Updated:** October 19, 2025  
**For Questions:** Contact Development Team