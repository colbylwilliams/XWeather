//
//  WeatherPvc.swift
//  XWeather
//
//  Created by Colby Williams on 9/13/16.
//  Copyright © 2016 Colby Williams. All rights reserved.
//

import Foundation
import UIKit

class WeatherPvc : UIPageViewController, UIPageViewControllerDelegate, UIPageViewControllerDataSource {
	
	@IBOutlet weak var emptyView: UIView!
	@IBOutlet weak var loadingContainerView: UIView!
	@IBOutlet weak var loadingIndicatorView: UIActivityIndicatorView!
	@IBOutlet weak var pageIndicator: UIPageControl!
	@IBOutlet var toolbarButtons: [UIButton]!
	@IBOutlet weak var toolbarView: UIView!
	
	
	var Controllers: [UITableViewController] = []
	
	override func viewDidLoad() {
		super.viewDidLoad()
		
		initToolbar()
		initControllers()
	}
	
	
	override func viewWillAppear(_ animated: Bool) {
		super.viewWillAppear(animated)
		
		updateToolbarButtons (true)
	}
	
	
	override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
		
		updateToolbarButtons(false)
	}
	
	
	override var preferredStatusBarStyle: UIStatusBarStyle {
		
		return .lightContent
	}

	
	@IBAction func closeClicked(_ sender: AnyObject) {
		
		updateToolbarButtons(true)
		
		dismiss(animated: true, completion: nil)
	}
	
	
	@IBAction func settingsClicked(_ sender: AnyObject) {
		
		if let settingsUrl = URL(string: UIApplicationOpenSettingsURLString) {
			
			if #available(iOS 10.0, *) {
			
				UIApplication.shared.open(settingsUrl, options: [:]) {
					completion in print(completion)
					// TrackEvent (TrackedEvents.Settings.Opened);
				}
			
			} else if UIApplication.shared.openURL(settingsUrl) {
				
				// TrackEvent (TrackedEvents.Settings.Opened);
			
			}
		}
	}


	func updateToolbarButtons(_ dismissing: Bool) {
	
		for button in toolbarButtons {
			button.isHidden = dismissing ? button.tag > 1 : button.tag < 2
		}
		
		pageIndicator.isHidden = !dismissing
	}

	
	func handleUpdatedCurrent() {
		
	}
	
	
	func reloadData() {
		
		updateBackground()
		
		for controller in Controllers {
			controller.tableView.reloadData()
		}
	}

	
	func updateBackground() {
		
	}

	
	func initControllers() {
		
		Controllers = [ storyboard?.instantiateViewController(withIdentifier: "DailyTvc") as! UITableViewController,
		                storyboard?.instantiateViewController(withIdentifier: "HourlyTvc") as! UITableViewController,
		                storyboard?.instantiateViewController(withIdentifier: "DetailsTvc") as! UITableViewController ]
		
		setViewControllers([Controllers[0] /* Settings.WeatherPage */], direction: .forward, animated: false) {
			completion in self.getData()
		}
		
		pageIndicator.currentPage = 0 // Settings.WeatherPage
		
		updateBackground()
	}
	
	
	// MARK: UIPageViewControllerDelegate
	
	func pageViewController(_ pageViewController: UIPageViewController, willTransitionTo pendingViewControllers: [UIViewController]) {
		
		updateBackground()
	}
	
	
	func pageViewController(_ pageViewController: UIPageViewController, didFinishAnimating finished: Bool, previousViewControllers: [UIViewController], transitionCompleted completed: Bool) {
		
		if let index = Controllers.index(of: viewControllers?[0] as! UITableViewController) {
			
			pageIndicator.currentPage = index
			
			// Settings.WeatherPage = index;
		}
	}
	
	
	// MARK: UIPageViewControllerDataSource
	
	func pageViewController(_ pageViewController: UIPageViewController, viewControllerAfter viewController: UIViewController) -> UIViewController? {
		
		return viewController.isEqual(Controllers[0]) ? Controllers [1] : viewController.isEqual(Controllers [1]) ? Controllers [2] : nil;
	}

	
	func pageViewController(_ pageViewController: UIPageViewController, viewControllerBefore viewController: UIViewController) -> UIViewController? {
		
		return viewController.isEqual(Controllers [2]) ? Controllers [1] : viewController.isEqual(Controllers [1]) ? Controllers [0] : nil;
	}

	
	func initToolbar() {
		
		toolbarView.translatesAutoresizingMaskIntoConstraints = false
		
		navigationController?.view.addSubview(toolbarView)
		
		navigationController?.view.addConstraints(NSLayoutConstraint.constraints(withVisualFormat: "H:|[toolbarView]|", options: .alignmentMask, metrics: nil, views: ["toolbarView" : toolbarView]))
		navigationController?.view.addConstraints(NSLayoutConstraint.constraints(withVisualFormat: "V:[toolbarView(44.0)]|", options: .alignmentMask, metrics: nil, views: ["toolbarView" : toolbarView]))
	}
	

	func getData() {
		
		UIApplication.shared.isNetworkActivityIndicatorVisible = true
		
		// get data
		
		UIApplication.shared.isNetworkActivityIndicatorVisible = false
	}
	
}
