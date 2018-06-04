//
//  ViewController.swift
//  RatesScheduleClient
//
//  Created by awu on 6/1/18.
//  Copyright © 2018 awu. All rights reserved.
//

import UIKit

class ViewController: UIViewController {
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
    
    @IBAction func submitDates(_ sender: Any) {
        if (validateDates()) {
            let alert = UIAlertController(title: "Price", message: "Fake Price", preferredStyle:.alert)
            alert.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: "Default action"), style: .default, handler: nil))
            self.present(alert, animated: true, completion: nil)
        } else {
            let alert = UIAlertController(title: "Invalid Times", message: "Your start and end times are invalid", preferredStyle:.alert)
            alert.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: "Default action"), style: .default, handler: nil))
            self.present(alert, animated: true, completion: nil)
        }
    }
}

