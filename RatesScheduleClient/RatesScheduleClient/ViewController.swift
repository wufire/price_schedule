//
//  ViewController.swift
//  RatesScheduleClient
//
//  Created by awu on 6/1/18.
//  Copyright © 2018 awu. All rights reserved.
//

import UIKit

class ViewController: UIViewController {
    
    // Change this base URL to your RatesSchedule server instance
    private let baseUrl = URL(string: "http://localhost:5000/")!
    private let SERVER_DATE_FORMAT_STRING = "yyyy-MM-dd'T'HH:mm:ss'Z'"
    
    @IBOutlet weak var startTimeTextField: UITextField!
    @IBOutlet weak var endTimeTextField: UITextField!
    
    let datePicker = UIDatePicker()
    let doneButton = UIBarButtonItem(barButtonSystemItem: .done, target: nil, action: #selector(dateDonePressed(sender:)))
    
    let ONE_HOUR = 1.0 * 60.0 * 60.0
    
    var startTime = Date()
    var endTime = Date()
    var canSubmit = Bool()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        datePicker.datePickerMode = UIDatePickerMode.dateAndTime
        datePicker.timeZone = NSTimeZone.system;
        datePicker.addTarget(self, action: #selector(textFieldValueChange(_:)), for: UIControlEvents.valueChanged)
        datePicker.tag = 0
        
        let toolbar = UIToolbar()
        toolbar.sizeToFit()
        
        doneButton.tag = 0
        toolbar.setItems([doneButton], animated: false)
        
        startTimeTextField.inputAccessoryView = toolbar
        startTimeTextField.inputView = datePicker
        startTimeTextField.layer.borderWidth = 1.0
        startTimeTextField.layer.borderColor = UIColor.black.cgColor
        
        endTimeTextField.inputAccessoryView = toolbar
        endTimeTextField.inputView = datePicker
        endTimeTextField.layer.borderWidth = 1.0
        endTimeTextField.layer.borderColor = UIColor.black.cgColor
    }
    
    @objc func dateDonePressed(sender: UIBarButtonItem) {
        if (sender.tag == 0) {
            startTime = datePicker.date
            startTimeTextField.text = formatDateForDisplay(date: datePicker.date)
        } else if (sender.tag == 1) {
            endTime = datePicker.date
            endTimeTextField.text = formatDateForDisplay(date: datePicker.date)
            
            canSubmit = validateDates()
        }
        self.view.endEditing(true)
    }

    @IBAction func submitDates(_ sender: Any) {
        if (validateDates()) {
            let jsonObject: [String: String] = [
                "starttime": formatDateForServer(date: startTime),
                "endtime": formatDateForServer(date: endTime)
            ]
            do {
                let dateData = try JSONSerialization.data(withJSONObject: jsonObject, options: JSONSerialization.WritingOptions())
                getPrice(jsonData: dateData)
            }
            catch {
                showSimpleAlert(title: "Parsing Error", message: "Encountered an error formatting data for the server. This doesn't really help you much, enduser")
            }
        } else {
            showSimpleAlert(title: "Invalid Times", message: "Your requested times are invalid. Please correct them and try again.")
        }
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    // TextField Delegates
    @IBAction func textFieldEditBegin(_ sender: UITextField) {
        if (sender === startTimeTextField) {
            datePicker.tag = 0
            datePicker.setDate(startTime, animated: true)
        } else if (sender === endTimeTextField) {
            datePicker.tag = 1
            if (endTimeTextField.text == "") {
                // Add one hour to the date for endTime date convenience
                datePicker.setDate(
                    startTime.addingTimeInterval(ONE_HOUR), animated: false
                )
            } else {
                datePicker.setDate(endTime, animated: true)
            }
        }
        doneButton.tag = datePicker.tag
    }
    
    @IBAction func textFieldValueChange(_ sender: UIDatePicker) {
        if (sender.tag == 0) {
            startTime = datePicker.date
            startTimeTextField.text = formatDateForDisplay(date: datePicker.date)
        } else if (sender.tag == 1) {
            endTime = datePicker.date
            endTimeTextField.text = formatDateForDisplay(date: datePicker.date)
            
            canSubmit = validateDates()
        }
    }
    
    // Helper functions
    func formatDateForServer(date: Date) -> String {
        return formatDate(date: date, format: SERVER_DATE_FORMAT_STRING)
    }
    
    func formatDate(date: Date, format: String) -> String {
        let formatter = DateFormatter()
        formatter.dateFormat = format
        
        return formatter.string(from: date)
    }
    
    func formatDateForDisplay(date: Date) -> String {
        let formatter = DateFormatter()
        formatter.dateStyle = DateFormatter.Style.short
        formatter.timeStyle = DateFormatter.Style.short
        formatter.doesRelativeDateFormatting = true
        return formatter.string(from: date)
    }
    
    func validateDates() -> Bool {
        if (endTime <= startTime) {
            endTimeTextField.layer.borderColor = UIColor.red.cgColor
            return false
        } else {
            endTimeTextField.layer.borderColor = UIColor.black.cgColor
            return true
        }
    }
    
    func showSimpleAlert(title: String, message: String) {
        let alert = UIAlertController(title: title, message: message, preferredStyle:.alert)
        alert.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: "Default action"), style: .default, handler: nil))
        self.present(alert, animated: true, completion: nil)
    }
    
    func getPrice(jsonData: Data) {
        let url = baseUrl.appendingPathComponent("api/rates/rateprice")
        
        if !jsonData.isEmpty {
            var request = URLRequest(url: url)
            request.httpMethod = "POST"
            request.httpBody = jsonData
            request.addValue("application/json", forHTTPHeaderField: "Content-Type")
            
            let task = URLSession.shared.dataTask(with: request, completionHandler:
                {
                    (responseData: Data?, response: URLResponse?, error: Error?) in
                    let responseString = String(data: responseData!, encoding: String.Encoding.utf8) as String?
                    
                    DispatchQueue.main.async(execute:{
                        self.showSimpleAlert(title: "Price", message: "The price for the selected time is \(responseString ?? "not found").")
                    })
                }
            )
            task.resume()
        }
    }
}

