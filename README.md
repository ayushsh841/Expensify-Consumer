# Expensify-Consumer
Two part service for creating jobs to the Expensify API and save the Job Result in DB

# Problem Statement
Read data from the Expensify API and save the details in Mongo DB

# Approach
The codebase is a two part solution:
  1. ExpenseColletor: The project is a service that is triggered after a duration to call the Expensify API and create a job to get the details.
  2. FileParser: The project is a service that runs on the SMTP Server which looks up for any new files to process and save the data into MongoDB.
  
# DB Models 
Vendor 
- id
- Name

Expense 
- id
- VendorId
- Cost
- ConvertedAmount
- Currency
- ModifiedAmount
- TaxAmount
- Category
- Type

Note: Details about expensify (https://integrations.expensify.com/Integration-Server/doc/#introduction)
