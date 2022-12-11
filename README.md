# ExamCandidateTracker-Web-API

- The appSettings.json contains some environment variables like 
    - Database connection (Microsoft SQL Server)
    - Email Notifications
- The Iron Bar Code QRCode free licence used in the project will Expire by January 2023 which will result error unless renewed.

# Exam-Candidates-Tracker-Descriptions

Jamb is about to conduct their exams, and then they want to stop having to print out their excel sheet and check those lists when people come on the exam day. They want to be able to use more digital forms of doing it. The scenario now is that they have an excel sheet that has the following details, as listed below:

    - Full Name
    - Email
    - Phone number
    - Gender
    - Exam number (e.g format 76594543GH  must be unique)
    - Exam date and time
    - Centre Code (e.g format EC2354)
The system we want allows us to pass the excel sheet to the system (UploadCandidatesFile endpoint), which then extracts the data from the sheet and automatically populates the Candidates table with columns similar to those in the excel sheet.

For example, if there are 10 candidates in the Excel sheet, it will loop through them once it is passed to the endpoint. For each candidate it gets to, once it's done saving all the details about that candidate in the table, it picks the candidate's email and generates a QR Code that will hold

    - Exam number
    - Center Code
Once the QR code is generated, it's sent to the candidate's email. So immediately after it's uploaded, every candidate will begin to get their QR code. So on the exam day, the candidate has to come with the QR Code. So instead of having to check the list again on that day, every candidate will have to present his or her QR Code. We will have VerifyCandidateQrCode endpoint where we will send the candidate's details in the QR Code as follows:

    - Exam number 
    - Center Code
which will determine whether or not the exam number exists; if it does, it will determine whether or not the center is the actual center.The candidate's attendance is recorded as follows:

    - Date and time
    - Exam number

- I added a GetAllCandidates endpoint which will help view all uploaded candidates and DeleteAllCandidate endpoint if the system wants to be reset for another use.
