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
        
        endTimeTextField.inputAccessoryView = toolbar
        endTimeTextField.inputView = datePicker
    }
    
    @objc func dateDonePressed(sender: UIBarButtonItem) {
        if (sender.tag == 0) {
            startTimeTextField.text = formatDate(date: datePicker.date)
            datePicker.setDate(
                datePicker.date.addingTimeInterval(
                    1.0 * 60.0 * 60.0
                ), animated: false
            )
        } else if (sender.tag == 1) {
            endTimeTextField.text = formatDate(date: datePicker.date)
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
    
    func formatDate(date: Date) -> String {
        let formatter = DateFormatter()
        formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'"
        
        return formatter.string(from: date)
    }
    
    @IBAction func submitDates(_ sender: Any) {
        
    }
}

