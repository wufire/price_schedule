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
    
    @IBOutlet weak var startTimeTextField: UITextField!
    @IBOutlet weak var endTimeTextField: UITextField!
    
    let datePicker = UIDatePicker()
    let doneButton = UIBarButtonItem(barButtonSystemItem: .done, target: nil, action: #selector(dateDonePressed(sender:)))
    
    var startTime = Date()
    var endTime = Date()
    var canSubmit = Bool()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        datePicker.datePickerMode = UIDatePickerMode.dateAndTime
        datePicker.timeZone = NSTimeZone.system;
        
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
            startTimeTextField.text = formatDate(date: datePicker.date)
            datePicker.setDate(
                datePicker.date.addingTimeInterval(
                    1.0 * 60.0 * 60.0
                ), animated: false
            )
        } else if (sender.tag == 1) {
            endTime = datePicker.date
            endTimeTextField.text = formatDate(date: datePicker.date)
            
            canSubmit = validateDates()
        }
        self.view.endEditing(true)
    }

    @IBAction func submitDates(_ sender: Any) {
        if (validateDates()) {
            let jsonObject: [String: String] = [
                "starttime": formatDate(date: startTime),
                "endtime": formatDate(date: endTime)
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
    @IBAction func textFieldTouchUpInside(_ sender: UITextField) {
        if (sender === startTimeTextField) {
            doneButton.tag = 0
        } else if (sender === endTimeTextField) {
            doneButton.tag = 1
        }
    }
    
    // Helper functions
    func formatDate(date: Date) -> String {
        let formatter = DateFormatter()
        formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'"
        
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
                    
                    self.showSimpleAlert(title: "Price", message: "The price for the selected time is \(responseString ?? "not found").")
                }
            )
            task.resume()
        }
    }
}

