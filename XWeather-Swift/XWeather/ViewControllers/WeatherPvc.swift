//
//  WeatherPvc.swift
//  XWeather
//
//  Created by Colby Williams on 6/11/17.
//  Copyright © 2017 Colby Williams. All rights reserved.
//

import UIKit

class WeatherPvc: UIPageViewController, UIPageViewControllerDataSource, UIPageViewControllerDelegate {

	@IBOutlet var emptyView: UIView!
	@IBOutlet var toolbarView: UIView!
	@IBOutlet weak var loadingContainerView: UIView!
	@IBOutlet weak var loadingIndicatorView: UIActivityIndicatorView!
	@IBOutlet weak var pageIndicator: UIPageControl!
	@IBOutlet var toolbarButtons: [UIButton]!
	
	var controllers = [UITableViewController]()
	
	
	override func viewDidLoad() {
        super.viewDidLoad()

        initToolbar()
		
		initControllers()
    }
	
	override func viewWillAppear(_ animated: Bool) {
		super.viewWillAppear(animated)
		
		updateToolbarButtons(true)
	}
	
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
	
	
	@IBAction func closeClicked(_ sender: Any) {
	}
	
	@IBAction func settingsClicked(_ sender: Any) {
	}
	
	
	func updateToolbarButtons (_ dismissing: Bool) {
		
		for button in toolbarButtons {
			button.isHidden = dismissing ? button.tag > 1 : button.tag < 2
		}
		
		pageIndicator.isHidden = !dismissing
	}
	
	
	func reloadData () {
		
		updateBackground()
		
		for controller in controllers {
			controller.tableView.reloadData()
		}
	}
	
	
	func updateBackground() {
		
		let location = WuClient.Shared.selected
		
		let random = location == nil || Settings.randomBackgrounds
		
		var layer = view.layer.sublayers?[0] as? CAGradientLayer
		
		if layer == nil {
			layer = CAGradientLayer()
			layer!.frame = view.bounds
			view.layer.insertSublayer(layer!, at: 0)
		}
		
		let gradients = location?.timeOfDayIndex (random);
		
		if let colors = gradients?.colors {
			
			if let oldColor0 = layer?.colors?[0], let oldColor1 = layer?.colors?[1] {
				if oldColor0 as! CGColor == colors[0] && oldColor1 as! CGColor == colors[1] {
					return
				}
			}
			
			CATransaction.begin()
			CATransaction.setAnimationDuration(1.5)
			layer?.colors = colors
			CATransaction.commit ()
		}
	}
	
	
	func initControllers() {
		
		if let dailyTvc = storyboard?.instantiateViewController(withIdentifier: "DailyTvc") as? UITableViewController,
			let hourlyTvc = storyboard?.instantiateViewController(withIdentifier: "HourlyTvc") as? UITableViewController,
			let detailsTvc = storyboard?.instantiateViewController(withIdentifier: "DetailsTvc") as? UITableViewController {
			
			controllers += [ dailyTvc, hourlyTvc, detailsTvc ]
		}
	}
	
	
	func initToolbar() {
		
		toolbarView.translatesAutoresizingMaskIntoConstraints = false
		
		navigationController?.view.addSubview(toolbarView)
		
		navigationController?.view.addConstraints(NSLayoutConstraint.constraints(withVisualFormat: "H:|[toolbarView]|", options: .directionLeadingToTrailing, metrics: [:], views: ["toolbarView": toolbarView]))
		navigationController?.view.addConstraints(NSLayoutConstraint.constraints(withVisualFormat: "V:[toolbarView(44.0)]|", options: .directionLeadingToTrailing, metrics: [:], views: ["toolbarView": toolbarView]))
	}
	
	
	
	// MARK: - UIPageViewControllerDataSource
	
	func pageViewController(_ pageViewController: UIPageViewController, viewControllerBefore viewController: UIViewController) -> UIViewController? {
		return pageViewController.isEqual(controllers[2]) ? controllers[1] : pageViewController.isEqual(controllers[1]) ? controllers[0] : nil
	}
	
	func pageViewController(_ pageViewController: UIPageViewController, viewControllerAfter viewController: UIViewController) -> UIViewController? {
		return pageViewController.isEqual(controllers[0]) ? controllers[1] : pageViewController.isEqual(controllers[1]) ? controllers[2] : nil
	}
	
	
	// MARK: - UIPageViewControllerDelegate
	
	func pageViewController(_ pageViewController: UIPageViewController, willTransitionTo pendingViewControllers: [UIViewController]) {
		updateBackground()
	}
	
	func pageViewController(_ pageViewController: UIPageViewController, didFinishAnimating finished: Bool, previousViewControllers: [UIViewController], transitionCompleted completed: Bool) {
		if let indexController = pageViewController.viewControllers?[0] as? UITableViewController, let index = controllers.index(of: indexController) {
			pageIndicator.currentPage = index
			Settings.weatherPage = index
		}
	}
}
